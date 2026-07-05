using System.Collections.Generic;

namespace Domain
{
    public class Status
    {
        public byte Id { get; set; }       // TINYINT
        public string Name { get; set; }
        public bool IsClosed { get; set; } // da li se status smatra zatvorenim

        public virtual HashSet<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
