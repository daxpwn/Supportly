using Application;
using Application.DTO;
using Application.DTO.Search;
using Application.Queries;
using Domain.Authorization;
using Supportly.DataAccess;
using System.Linq;

namespace Implementation.UseCases.Queries
{
    public class EfGetMyTicketsQuery : EfUseCase, IGetMyTicketsQuery
    {
        private readonly IApplicationUser _user;

        public EfGetMyTicketsQuery(LabDbContext context, IApplicationUser user) : base(context)
        {
            _user = user;
        }

        public string Name => "Get my tickets (requester = current user)";

        public string Id => UseCaseIds.GetMyTickets;

        public PagedResponse<TicketListItemDTO> Execute(TicketSearch search)
        {
            int page = search.Page ?? 1;
            int perPage = search.PerPage ?? 10;
            if (page < 1) page = 1;
            if (perPage < 1) perPage = 10;
            if (perPage > 100) perPage = 100;

            // SAMO tiketi ulogovanog korisnika (on je Requester)
            var query = ctx.Tickets.Where(t => t.RequesterId == _user.Id);

            if (!string.IsNullOrWhiteSpace(search.Keyword))
                query = query.Where(t => t.Subject.Contains(search.Keyword)
                                      || t.TicketNumber.Contains(search.Keyword));

            if (search.StatusId.HasValue)
                query = query.Where(t => t.StatusId == search.StatusId.Value);

            if (search.PriorityId.HasValue)
                query = query.Where(t => t.PriorityId == search.PriorityId.Value);

            if (search.CategoryId.HasValue)
                query = query.Where(t => t.CategoryId == search.CategoryId.Value);

            if (search.CreatedFrom.HasValue)
                query = query.Where(t => t.CreatedAt >= search.CreatedFrom.Value);

            if (search.CreatedTo.HasValue)
                query = query.Where(t => t.CreatedAt <= search.CreatedTo.Value);

            int totalCount = query.Count();

            var items = query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .Select(t => new TicketListItemDTO
                {
                    Id = t.Id,
                    TicketNumber = t.TicketNumber,
                    Subject = t.Subject,
                    Status = t.Status.Name,
                    Priority = t.Priority.Name,
                    CreatedAt = t.CreatedAt
                })
                .ToList();

            return new PagedResponse<TicketListItemDTO>
            {
                Items = items,
                TotalCount = totalCount,
                CurrentPage = page,
                PerPage = perPage
            };
        }
    }
}
