using System;
using System.Collections.Generic;

namespace InventoryWebAPI.Models
{
    public partial class User
    {
        public User()
        {          
             Invoices = new HashSet<Invoice>();
        }

        public int UserId { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string Source { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public short RoleId { get; set; }
        public DateTime? HireDate { get; set; }

        public virtual Role Role { get; set; }
        public virtual ICollection<Invoice> Invoices { get; set; }

    }
}
