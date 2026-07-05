using Application.DTO;
using Application.DTO.Search;
using Application.Queries;
using Domain.Authorization;
using Supportly.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Implementation.UseCases.Queries
{
    public class EfGetStatusesQuery : EfUseCase, IGetStatusesQuery
    {
        public EfGetStatusesQuery(LabDbContext context) : base(context)
        {
        }

        public string Name => "Vraca sve statuse";

        public string Id => UseCaseIds.GetStatuses;

        public IEnumerable<StatusDTO> Execute(StatusSearch search)
        {
            return ctx.Statuses
                      .Select(s => new StatusDTO
                      {
                          Id = s.Id,
                          Name = s.Name,
                          IsClosed = s.IsClosed
                      })
                      .ToList();
        }
    }
}
