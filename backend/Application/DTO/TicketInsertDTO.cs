using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Application.DTO
{
    public class TicketInsertDTO
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public int? DepartmentId { get; set; }
        public int? CategoryId { get; set; }
        public int PriorityId { get; set; }
        public int StatusId { get; set; }
        public DateTime? DueAt { get; set; }
        public List<int> AttachmentIds { get; set; }

        // Izlaz: popunjava ih komanda posle upisa (id/broj kreiranog tiketa).
        public long Id { get; set; }
        public string TicketNumber { get; set; }
    }
}
