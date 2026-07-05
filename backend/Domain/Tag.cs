using System.Collections.Generic;

namespace Domain
{
    public class Tag
    {
        public short Id { get; set; }    // SMALLINT
        public string Name { get; set; }

        public virtual HashSet<TicketTag> TicketTags { get; set; } = new HashSet<TicketTag>();
    }
}
