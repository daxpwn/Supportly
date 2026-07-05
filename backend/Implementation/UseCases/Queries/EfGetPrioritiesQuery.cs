using Application.DTO;
using Application.DTO.Search;
using Application.Queries;
using Domain.Authorization;
using Supportly.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Implementation.UseCases.Queries
{
    public class EfGetPrioritiesQuery : EfUseCase, IGetPrioritiesQuery
    {
        public EfGetPrioritiesQuery(LabDbContext context) : base(context)
        {
        }

        public string Name => "Vraca sve prioritete";

        public string Id => UseCaseIds.GetPriorities;

        public IEnumerable<PriorityDTO> Execute(PrioritySearch search)
        {
            return ctx.Priorities
                      .OrderBy(p => p.Level)
                      .Select(p => new PriorityDTO
                      {
                          Id = p.Id,
                          Name = p.Name,
                          Level = p.Level
                      })
                      .ToList();
        }
    }
}
