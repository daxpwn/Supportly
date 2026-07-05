using System;

namespace Domain
{
    // Prilozi (vezani za tiket i opciono za konkretan komentar)
    public class Attachment
    {
        public long Id { get; set; }          // BIGINT
        public long TicketId { get; set; }
        public long? CommentId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string MimeType { get; set; }
        public int? FileSize { get; set; }    // u bajtovima
        public int UploadedBy { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual TicketComment Comment { get; set; }
        public virtual User Uploader { get; set; }
    }
}
