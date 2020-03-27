using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ContactInfo.Models
{
    [Table("Contact")]    
    public class Contact
    {
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        [Key]
        public virtual string Email { get; set; }
        public virtual long PhoneNumber { get; set; }
        public virtual EnumStatus Status { get; set; }
    }
}