using System.Collections.Generic;

namespace Domain
{
    public class Priority
    {
        public byte Id { get; set; }      // TINYINT
        public string Name { get; set; }
        public byte Level { get; set; }   // 1 = najniži ... (CHECK 1..10)

        public virtual HashSet<Ticket> Tickets { get; set; } = new HashSet<Ticket>();
    }
}
