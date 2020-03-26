using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using ContactInfo.EFModel;
using ContactInfo.Models;

namespace ContactInfo.Repository
{
    public class ContactRepository : IContactRepository
    {       
        ContactDBContext _contactDBContext;
        public ContactRepository(ContactDBContext contactDBContext)
        {
            _contactDBContext = contactDBContext;
        }

        public void AddContacts([FromBody] Contact contactInfo)
        {            
           _contactDBContext.Contacts.Add(contactInfo);
           _contactDBContext.SaveChanges();  
        }

        public Contact EditContact([FromBody] Contact contactInfo)
        {
            _contactDBContext.Entry(contactInfo).State = System.Data.Entity.EntityState.Modified;
            _contactDBContext.SaveChanges();
            return contactInfo;
        }

        public List<Contact> GetContacts()
        {
            return _contactDBContext.Contacts.Where(x=>x.Status == EnumStatus.Active).ToList<Contact>();
        }

        public bool DeleteContact([FromBody] Contact contactInfo, bool isDelete = false, bool isInActive = false)
        {
            try
            {
                if (isInActive)
                {
                    Contact contactDetails = _contactDBContext.Contacts.Where(x => x.Email.Equals(contactInfo.Email, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    contactDetails.Status = EnumStatus.InActive;                    
                }
                else if(isDelete)
                {
                  Contact contact =  _contactDBContext.Contacts.Where(x => x.Email.Equals(contactInfo.Email, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                    _contactDBContext.Contacts.Remove(contact);
                }
                _contactDBContext.SaveChanges();
                return true;

            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}