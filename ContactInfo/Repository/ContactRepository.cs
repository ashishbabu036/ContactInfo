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
            if (contactDBContext == null)
                throw new NullReferenceException();
            _contactDBContext = contactDBContext;
        }

        public bool AddContacts([FromBody] Contact contactInfo)
        {
            if (!_contactDBContext.Contacts.Where(x => x.Email.Equals(contactInfo.Email, StringComparison.OrdinalIgnoreCase)).Any())
            {
                _contactDBContext.Contacts.Add(contactInfo);
                _contactDBContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
           
        }

        public Contact EditContact([FromBody] Contact contactInfo)
        {
            SetEntityStateModified(contactInfo);
            _contactDBContext.SaveChanges();
            return contactInfo;
        }

        public virtual void SetEntityStateModified(Contact contactInfo)
        {
            _contactDBContext.Entry(contactInfo).State = System.Data.Entity.EntityState.Modified;
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