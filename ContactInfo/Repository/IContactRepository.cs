using ContactInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContactInfo.Repository
{
    public interface IContactRepository
    {
        List<Contact> GetContacts();

        void AddContacts(Contact contactInfo);

        Contact EditContact(Contact contactInfo);

        bool DeleteContact(Contact contactInfo, bool isDelete, bool isInActive);
    }
}
