using System;

namespace Domain
{
    // Istorija promena (audit log) na tiketu
    public class TicketHistory
    {
        public long Id { get; set; }          // BIGINT
        public long TicketId { get; set; }
        public int ChangedBy { get; set; }
        public string FieldName { get; set; } // npr. 'StatusId', 'AssigneeId'
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public DateTime ChangedAt { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual User ChangedByUser { get; set; }
    }
}
