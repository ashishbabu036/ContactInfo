using Autofac.Extras.Moq;
using ContactInfo.EFModel;
using ContactInfo.Models;
using ContactInfo.Repository;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using Xunit;

namespace ContactInfo.Tests
{
    public class ContactRepositoryTests
    {
        IQueryable<Contact> queryable;

        public IQueryable<Contact> Queryable { get => queryable; set => queryable = value; }

        public Mock<DbSet<ContactInfo.Models.Contact>> SetUpDbSet()
        {
             Queryable = new List<Contact>
             {
               new Contact() { FirstName="Ashish",LastName="Babu",PhoneNumber=9545027451,Email="ashishhbabu036@yahoo.com",Status=EnumStatus.Active },
               new Contact() { FirstName="Rahul",LastName="Byelle",PhoneNumber=9158915645,Email="rahull.byelle@yahoo.com",Status=EnumStatus.Active }
             }.AsQueryable();
           
            var dbSet = new Mock<DbSet<ContactInfo.Models.Contact>>();
            dbSet.As<IQueryable<ContactInfo.Models.Contact>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<ContactInfo.Models.Contact>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<ContactInfo.Models.Contact>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<ContactInfo.Models.Contact>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            return dbSet;
        }

        [Fact]
        public void Repository_ContextNullReference()
        {           
            Assert.Throws<NullReferenceException>(() => new ContactRepository(null));
        }

        [Fact]
        public void GetContacts_PositiveFlow()
        {
            var dbSet = SetUpDbSet();
            var contactDBContext = new Mock<ContactDBContext>();
            var contactsMock = new Mock<DbSet<Contact>>();
            contactDBContext.Setup(x => x.Contacts).Returns(dbSet.Object);          

            var ctr = new ContactRepository(contactDBContext.Object);
            var actual = ctr.GetContacts() as List<Contact>;
            Assert.NotNull(actual);
            Assert.Equal(queryable.Count(), actual.Count);
        }

        [Fact]
        public void GetContacts_NegativeFlow()
        {
            var dbSet = SetUpDbSet();
            var contactDBContext = new Mock<ContactDBContext>();
            var contactsMock = new Mock<DbSet<Contact>>();
            contactDBContext.Setup(x => x.Contacts).Throws(new NullReferenceException());           

            var ctr = new ContactRepository(contactDBContext.Object);            
            Assert.Throws<NullReferenceException>(()=>ctr.GetContacts());
        }

        [Fact]
        public void AddContacts_PositiveFlow()
        {
            var dbSet = SetUpDbSet();
            var contactDBContext = new Mock<ContactDBContext>();
            var contactsMock = new Mock<DbSet<Contact>>();
            contactDBContext.Setup(x => x.Contacts).Returns(dbSet.Object);
            contactsMock.Setup(x => x.Add(It.IsAny<Contact>())).Returns((Contact u) => u);

            var ctr = new ContactRepository(contactDBContext.Object);
            bool isResult = ctr.AddContacts(new ContactInfo.Models.Contact() { FirstName = "Ashish", LastName = "Babu", PhoneNumber = 9545027451, Email = "ashishbabu036@yahoo.com", Status = EnumStatus.Active });
            Assert.True(isResult);
        }

        [Fact]
        public void AddContacts_NegativeFlow()
        {
            var dbSet = SetUpDbSet();
            var contactDBContext = new Mock<ContactDBContext>();
            var contactsMock = new Mock<DbSet<Contact>>();
            contactsMock.Setup(x => x.Add(It.IsAny<Contact>())).Throws(new NullReferenceException());

            var ctr = new ContactRepository(contactDBContext.Object);
            Assert.Throws<ArgumentNullException>(()=> ctr.AddContacts(new ContactInfo.Models.Contact() { FirstName = "Ashish", LastName = "Babu", PhoneNumber = 9545027451, Email = "ashishbabu036@yahoo.com", Status = EnumStatus.Active }));
        }

        [Fact]
        public void DeleteContacts_PositiveFlow()
        {
            var dbSet = SetUpDbSet();
            var contactDBContext = new Mock<ContactDBContext>();
            var contactsMock = new Mock<DbSet<Contact>>();
            contactDBContext.Setup(x => x.Contacts).Returns(dbSet.Object);                  

            var ctr = new ContactRepository(contactDBContext.Object);
            Contact editContact = new Contact() { FirstName = "Ashish", LastName = "Babu", PhoneNumber = 9546027451, Email = "ashishbabu036@yahoo.com", Status = EnumStatus.Active };
            var actual = ctr.DeleteContact(editContact, isDelete:true);
            Assert.True(actual);          
        }

        [Fact]
        public void DeActivateContacts_PositiveFlow()
        {
            var dbSet = SetUpDbSet();

            var contactDBContext = new Mock<ContactDBContext>();
            var contactsMock = new Mock<DbSet<Contact>>();
            contactDBContext.Setup(x => x.Contacts).Returns(dbSet.Object);             

            var ctr = new ContactRepository(contactDBContext.Object);
            Contact editContact = new Contact() { FirstName = "Rahul", LastName = "Byelle", PhoneNumber = 9546027451, Email = "rahull.byelle@yahoo.com", Status = EnumStatus.Active };
            var actual = ctr.DeleteContact(editContact, isInActive: true);
            Assert.True(actual);
        }

        [Fact]
        public void DeleteContacts_NegativeFlow()
        {
            var dbSet = SetUpDbSet();

            var contactDBContext = new Mock<ContactDBContext>();
            var contactsMock = new Mock<DbSet<Contact>>();
            contactDBContext.Setup(x => x.Contacts).Throws(new NullReferenceException());

            var ctr = new ContactRepository(contactDBContext.Object);
            Contact editContact = new Contact() { FirstName = "Ashish", LastName = "Babu", PhoneNumber = 9546027451, Email = "ashishbabu036@yahoo.com", Status = EnumStatus.Active };
            Assert.False(ctr.DeleteContact(editContact, isDelete: true));         
        }

        [Fact]
        public void EditContacts_PositiveFlow()
        {
            var dbSet = SetUpDbSet();
            var contactDBContext = new Mock<ContactDBContext>();
            var contactsMock = new Mock<DbSet<Contact>>();                   

            var ctr = new Mock<ContactRepository>(contactDBContext.Object);
            ctr.Setup(x => x.SetEntityStateModified(It.IsAny<Contact>()));
            Contact editContact = new Contact() { FirstName = "Ashish", LastName = "Babu", PhoneNumber = 9546027451, Email = "ashishbabu036@yahoo.com", Status = EnumStatus.Active };
            var actual = ctr.Object.EditContact(editContact);
            Assert.NotNull(actual);
        }

        [Fact]
        public void EditContacts_NegativeFlow()
        {
            var dbSet = SetUpDbSet();
            var contactDBContext = new Mock<ContactDBContext>();
            var contactsMock = new Mock<DbSet<Contact>>();

            var ctr = new Mock<ContactRepository>(contactDBContext.Object);
            ctr.Setup(x => x.SetEntityStateModified(It.IsAny<Contact>())).Throws(new Exception());
            Contact editContact = new Contact() { FirstName = "Ashish", LastName = "Babu", PhoneNumber = 9546027451, Email = "ashishbabu036@yahoo.com", Status = EnumStatus.Active };          
            Assert.Throws<Exception>(() => ctr.Object.EditContact(editContact));
        }
    }
}
