using System;
using System.Collections.Generic;

namespace Domain
{
    // Komentari / odgovori na tiket (uključuje i interne beleške)
    public class TicketComment
    {
        public long Id { get; set; }          // BIGINT
        public long TicketId { get; set; }
        public int AuthorId { get; set; }
        public string Body { get; set; }
        public bool IsInternal { get; set; }  // interna beleška, ne vidi je klijent
        public DateTime CreatedAt { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual User Author { get; set; }

        public virtual HashSet<Attachment> Attachments { get; set; } = new HashSet<Attachment>();
    }
}
