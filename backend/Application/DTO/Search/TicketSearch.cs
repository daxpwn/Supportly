using System;
using Application.DTO;

namespace Application.DTO.Search
{
    // Filteri za pretragu tiketa. Page/PerPage nasleđeni iz PagedSearch (default 1 / 10).
    public class TicketSearch : PagedSearch
    {
        public string Keyword { get; set; }        // traži po Subject / TicketNumber
        public int? StatusId { get; set; }
        public int? PriorityId { get; set; }
        public int? AssigneeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? CategoryId { get; set; }

        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }

        public bool? OnlyOpen { get; set; }         // true => samo tiketi čiji status nije zatvoren

        // Sortiranje: SortBy ∈ { createdAt (default), priority, status, subject, ticketNumber }
        // SortDir ∈ { desc (default), asc }
        public string SortBy { get; set; }
        public string SortDir { get; set; }
    }
}
