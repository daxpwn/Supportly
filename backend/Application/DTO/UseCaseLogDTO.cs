using System;

namespace Application.DTO
{
    public class UseCaseLogDTO
    {
        public long Id { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string UseCaseId { get; set; }
        public string UseCaseName { get; set; }
        public DateTime ExecutedAt { get; set; }
        public long DurationMs { get; set; }
        public bool Succeeded { get; set; }
        public string Payload { get; set; }
    }
}
