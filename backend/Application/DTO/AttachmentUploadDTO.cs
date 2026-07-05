namespace Application.DTO
{
    // Ulaz za upload priloga. Kontroler ga puni iz IFormFile (bez ASP.NET tipova ovde).
    public class AttachmentUploadDTO
    {
        public long TicketId { get; set; }
        public long? CommentId { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
    }
}
