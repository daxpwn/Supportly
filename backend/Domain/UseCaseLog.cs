using System;

namespace Domain
{
    // Audit log: jedan red po pokušaju izvršavanja use case-a
    // (ko, koji use case, kada, koliko je trajalo, i da li je uspeo).
    public class UseCaseLog
    {
        public long Id { get; set; }          // BIGINT
        public int? UserId { get; set; }      // null za gosta (neulogovan)
        public string Username { get; set; }
        public string UseCaseId { get; set; }
        public string UseCaseName { get; set; }
        public DateTime ExecutedAt { get; set; }
        public long DurationMs { get; set; }
        public bool Succeeded { get; set; }   // false = pokušaj pao (npr. nema dozvole / greška)
        public string Payload { get; set; }   // prosleđene vrednosti (JSON), npr. {"ticketId":5,"statusId":1}
    }
}
