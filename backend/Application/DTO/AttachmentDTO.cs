namespace Application.DTO
{
    public class AttachmentDTO
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }   // relativna putanja/URL (/Uploads/...)
        public string MimeType { get; set; }
    }
}
