using ContactInfo.Models;
using ContactInfo.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ContactInfo.Controllers
{
    [RoutePrefix("Contacts")]

    public class ContactsController : ApiController
    {
        private IContactRepository _contactInfo;
        public ContactsController(IContactRepository contactInfo)
        {
            _contactInfo = contactInfo;
        }

        [HttpGet]       
        public IHttpActionResult GetContacts()
        { 
            try
            {
               List<Contact> contactList = _contactInfo.GetContacts();              
               return Ok<List<Contact>>(contactList);               
            }
            catch(Exception ex)
            {
               return InternalServerError(ex);
            }
        }
          
        [HttpPost]
        [Route("AddContact")]
        public IHttpActionResult AddContact(Contact contact)
        {
            try
            {
                _contactInfo.AddContacts(contact);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPut]
        [Route("UpdateContact")]
        public IHttpActionResult UpdateContact(Contact contact)
        {
            try
            {
                contact = _contactInfo.EditContact(contact);
                return Ok<Contact>(contact);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpDelete]
        [Route("DeleteContact")]
        public IHttpActionResult DeleteContact(Contact contact, bool isDelete=false, bool isInActive=false)
        {
            try
            {
                bool isUpdated = _contactInfo.DeleteContact(contact, isDelete, isInActive);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}