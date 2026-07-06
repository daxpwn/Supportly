using System;
using Application.DTO;

namespace Application.DTO.Search
{
    // Filteri za pretragu audit loga. Page/PerPage nasleđeni iz PagedSearch (default 1 / 10).
    public class UseCaseLogSearch : PagedSearch
    {
        public int? UserId { get; set; }
        public string Username { get; set; }     // deo imena korisnika (Contains)
        public string UseCaseName { get; set; }  // deo naziva use case-a (Contains)

        public DateTime? From { get; set; }       // opseg datuma: od
        public DateTime? To { get; set; }         // opseg datuma: do
    }
}
