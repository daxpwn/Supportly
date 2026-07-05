using System;
using System.Collections.Generic;

namespace Domain
{
    public class Ticket
    {
        public long Id { get; set; }                  // BIGINT
        public string TicketNumber { get; set; }      // npr. 'HD-2026-000123'
        public string Subject { get; set; }
        public string Description { get; set; }

        public int RequesterId { get; set; }          // klijent koji je otvorio
        public int? AssigneeId { get; set; }          // agent zadužen za tiket
        public short? DepartmentId { get; set; }
        public short? CategoryId { get; set; }
        public byte PriorityId { get; set; }
        public byte StatusId { get; set; }

        public DateTime? DueAt { get; set; }
        public DateTime? FirstResponseAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public DateTime? ClosedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual User Requester { get; set; }
        public virtual User Assignee { get; set; }
        public virtual Department Department { get; set; }
        public virtual Category Category { get; set; }
        public virtual Priority Priority { get; set; }
        public virtual Status Status { get; set; }

        public virtual HashSet<TicketComment> Comments { get; set; } = new HashSet<TicketComment>();
        public virtual HashSet<Attachment> Attachments { get; set; } = new HashSet<Attachment>();
        public virtual HashSet<TicketTag> TicketTags { get; set; } = new HashSet<TicketTag>();
        public virtual HashSet<TicketHistory> History { get; set; } = new HashSet<TicketHistory>();
    }
}
