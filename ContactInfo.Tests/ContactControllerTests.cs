using Autofac.Extras.Moq;
using ContactInfo.Controllers;
using ContactInfo.Models;
using ContactInfo.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Results;
using Xunit;

namespace ContactInfo.Tests
{
    public class ContactControllerTests
    {
        [Fact]
        public void Controller_RepositoryNullReference()
        {
            Assert.Throws<NullReferenceException>(() => new ContactsController(null));
        }
        [Fact]
        public async void GetAllContacts_PositiveFlow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IContactRepository>()
                    .Setup(x => x.GetContacts())
                    .Returns(ContactsList());

                var ctr = mock.Create<ContactsController>();
                var expected = ContactsList();
                var actual = ctr.GetContacts() as OkNegotiatedContentResult<List<Contact>>;
                Assert.NotNull(actual);
                Assert.NotNull(actual.Content);
                Assert.Equal(expected.Count, actual.Content.Count);
            }
        }

        [Fact]
        public async void GetAllContacts_NegativeFlow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IContactRepository>()
                    .Setup(x => x.GetContacts())
                    .Throws(new NullReferenceException());

                var ctr = mock.Create<ContactsController>();                
                Assert.IsType<System.Web.Http.Results.ExceptionResult>(ctr.GetContacts());
            }
        }

        [Fact]
        public async void AddContact_PositiveFlow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IContactRepository>()
                    .Setup(x => x.AddContacts(It.IsAny<Contact>()))
                    .Returns(true);

                var ctr = mock.Create<ContactsController>();                
                var actual = ctr.AddContact(new Mock<Contact>().Object);                
                Assert.IsType<System.Web.Http.Results.OkResult> (actual);
            }
        }

        [Fact]
        public async void AddContact_ExceptionFlow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IContactRepository>()
                    .Setup(x => x.AddContacts(It.IsAny<Contact>()))
                     .Throws(new NullReferenceException());

                var ctr = mock.Create<ContactsController>();               
                Assert.IsType<System.Web.Http.Results.ExceptionResult>(ctr.AddContact(new Mock<Contact>().Object));
            }
        }

        [Fact]
        public async void AddContact_Fail()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IContactRepository>()
                    .Setup(x => x.AddContacts(It.IsAny<Contact>()))
                    .Returns(false);

                var ctr = mock.Create<ContactsController>();
                var actual = ctr.AddContact(new Mock<Contact>().Object);
                Assert.IsType<System.Web.Http.Results.InternalServerErrorResult>(ctr.AddContact(new Mock<Contact>().Object));
            }
        }

        [Fact]
        public async void DeleteContact_PositiveFlow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IContactRepository>()
                    .Setup(x => x.DeleteContact(It.IsAny<Contact>(), It.IsAny<bool>(), It.IsAny<bool>()))
                      .Throws(new Exception());

                var ctr = mock.Create<ContactsController>();
                var actual = ctr.DeleteContact(new Mock<Contact>().Object,isDelete:true);
                Assert.IsType<System.Web.Http.Results.ExceptionResult>(actual);
            }
        }

        [Fact]
        public async void DeActivateContact_PositiveFlow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IContactRepository>()
                    .Setup(x => x.DeleteContact(It.IsAny<Contact>(), It.IsAny<bool>(), It.IsAny<bool>()))
                    .Returns(true);

                var ctr = mock.Create<ContactsController>();
                var actual = ctr.DeleteContact(new Mock<Contact>().Object, isInActive: true);
                Assert.IsType<System.Web.Http.Results.OkNegotiatedContentResult<bool>>(actual);
            }
        }
       
        [Fact]
        public async void DeleteContact_NegativeFlow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IContactRepository>()
                    .Setup(x => x.DeleteContact(It.IsAny<Contact>(), It.IsAny<bool>(), It.IsAny<bool>()))
                    .Returns(true);

                var ctr = mock.Create<ContactsController>();
                var actual = ctr.DeleteContact(new Mock<Contact>().Object, isDelete: true);
                Assert.IsType<System.Web.Http.Results.OkNegotiatedContentResult<bool>>(actual);
            }
        }

        [Fact]
        public async void EditContact_PositiveFlow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IContactRepository>()
                    .Setup(x => x.EditContact(It.IsAny<Contact>()))
                    .Returns(new Mock<Contact>().Object);

                var ctr = mock.Create<ContactsController>();
                var actual = ctr.UpdateContact(new Mock<Contact>().Object);
                Assert.IsType<System.Web.Http.Results.OkNegotiatedContentResult<Contact>>(actual);
            }
        }

        [Fact]
        public async void EditContact_NegativeFlow()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IContactRepository>()
                    .Setup(x => x.EditContact(It.IsAny<Contact>()))
                   .Throws(new Exception());

                var ctr = mock.Create<ContactsController>();
                var actual = ctr.UpdateContact(new Mock<Contact>().Object);
                Assert.IsType<System.Web.Http.Results.ExceptionResult>(actual);
            }
        }

        private List<Contact> ContactsList()
        {
            return new List<Contact>()
            {
                new Contact() { FirstName="Ashish",LastName="Babu",PhoneNumber=9545027451,Email="ashishbabu036@yahoo.com",Status=EnumStatus.Active },
                new Contact() { FirstName="Rahul",LastName="Byelle",PhoneNumber=9158915645,Email="rahul.byelle@yahoo.com",Status=EnumStatus.Active }
            };
        }
    }
}
