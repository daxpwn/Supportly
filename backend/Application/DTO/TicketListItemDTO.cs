using System;

namespace Application.DTO
{
    public class TicketListItemDTO
    {
        public long Id { get; set; }
        public string TicketNumber { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }     // poslednji (trenutni) status tiketa
        public bool IsClosed { get; set; } 
        public string Priority { get; set; }
        public string Category { get; set; }
        public string Requester { get; set; }  // ime klijenta koji je otvorio tiket
        public string Assignee { get; set; }   // ime agenta zaduženog za tiket
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<TicketCommentsDTO> Comments { get; set; }
        public List<AttachmentDTO> Attachments { get; set; }
    }

    public class TicketCommentsDTO
    {
        public long Id { get; set; }
        public string Author { get; set; }
        public string Body { get; set; }
        public bool IsInternal { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<AttachmentDTO> Attachments { get; set; }
    }
}
