using Application.DTO;
using Application.DTO.Search;
using Application.Queries;
using Domain.Authorization;
using Supportly.DataAccess;
using System.Linq;

namespace Implementation.UseCases.Queries
{
    public class EfGetUseCaseLogsQuery : EfUseCase, IGetUseCaseLogsQuery
    {
        public EfGetUseCaseLogsQuery(LabDbContext context) : base(context)
        {
        }

        public string Name => "Get audit log (use case executions)";

        public string Id => UseCaseIds.GetUseCaseLogs;

        public PagedResponse<UseCaseLogDTO> Execute(UseCaseLogSearch search)
        {
            int page = search.Page ?? 1;
            int perPage = search.PerPage ?? 10;
            if (page < 1) page = 1;
            if (perPage < 1) perPage = 10;
            if (perPage > 100) perPage = 100;

            var query = ctx.UseCaseLogs.AsQueryable();

            if (search.UserId.HasValue)
                query = query.Where(l => l.UserId == search.UserId.Value);

            if (!string.IsNullOrWhiteSpace(search.Username))
                query = query.Where(l => l.Username.Contains(search.Username));

            if (!string.IsNullOrWhiteSpace(search.UseCaseName))
                query = query.Where(l => l.UseCaseName.Contains(search.UseCaseName));

            if (search.From.HasValue)
                query = query.Where(l => l.ExecutedAt >= search.From.Value);

            if (search.To.HasValue)
                query = query.Where(l => l.ExecutedAt <= search.To.Value);

            int totalCount = query.Count();

            var items = query
                .OrderByDescending(l => l.ExecutedAt)
                .ThenByDescending(l => l.Id)
                .Skip((page - 1) * perPage)
                .Take(perPage)
                .Select(l => new UseCaseLogDTO
                {
                    Id = l.Id,
                    UserId = l.UserId,
                    Username = l.Username,
                    UseCaseId = l.UseCaseId,
                    UseCaseName = l.UseCaseName,
                    ExecutedAt = l.ExecutedAt,
                    DurationMs = l.DurationMs,
                    Succeeded = l.Succeeded,
                    Payload = l.Payload
                })
                .ToList();

            return new PagedResponse<UseCaseLogDTO>
            {
                Items = items,
                TotalCount = totalCount,
                CurrentPage = page,
                PerPage = perPage
            };
        }
    }
}
