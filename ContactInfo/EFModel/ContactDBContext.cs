using ContactInfo.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ContactInfo.EFModel
{
    public class ContactDBContext : DbContext
    {
        public ContactDBContext() : base("name=DbContactConnectionString")
        {

        }
        public virtual DbSet<Contact> Contacts { get; set; }
    }
}