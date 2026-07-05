using System;
using System.Collections.Generic;

namespace Domain
{
    public class User : BaseEntity   // Id je INT
    {
        public byte RoleId { get; set; }
        public short? DepartmentId { get; set; }   // NULL za klijente
        public string FullName { get; set; }
        public string Email { get; set; }          // login identifikator (UNIQUE)
        public string PasswordHash { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual Role Role { get; set; }
        public virtual Department Department { get; set; }

        // Helpdesk navigacije
        public virtual HashSet<Ticket> RequestedTickets { get; set; } = new HashSet<Ticket>(); // Ticket.RequesterId
        public virtual HashSet<Ticket> AssignedTickets { get; set; } = new HashSet<Ticket>();  // Ticket.AssigneeId
        public virtual HashSet<TicketComment> Comments { get; set; } = new HashSet<TicketComment>();
        public virtual HashSet<Attachment> Attachments { get; set; } = new HashSet<Attachment>();
        public virtual HashSet<TicketHistory> HistoryChanges { get; set; } = new HashSet<TicketHistory>();
        public virtual HashSet<CannedResponse> CannedResponses { get; set; } = new HashSet<CannedResponse>();
    }
}
