using System;
using System.Collections.Generic;

namespace Domain
{
    public class Department
    {
        public short Id { get; set; }       // SMALLINT
        public string Name { get; set; }
        public string Email { get; set; }   // NULL dozvoljeno
        public DateTime CreatedAt { get; set; }

        public virtual HashSet<User> Users { get; set; } = new HashSet<User>();
        public virtual HashSet<Category> Categories { get; set; } = new HashSet<Category>();
        public virtual HashSet<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
