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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [Key]
        public string Email { get; set; }
        public long PhoneNumber { get; set; }
        public EnumStatus Status { get; set; }
    }
}