using Application.DTO;
using Application.Queries;
using Domain.Authorization;
using Supportly.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Implementation.UseCases.Queries
{
    public class EfGetRolesQuery : EfUseCase, IGetRolesQuery
    {
        public EfGetRolesQuery(LabDbContext context) : base(context)
        {
        }

        public string Name => "Get roles with their use cases";

        public string Id => UseCaseIds.ManageRoles;

        public IEnumerable<RoleWithUseCasesDTO> Execute(object request)
        {
            return ctx.Roles
                      .OrderBy(r => r.Id)
                      .Select(r => new RoleWithUseCasesDTO
                      {
                          Id = r.Id,
                          Name = r.Name,
                          Description = r.Description,
                          UseCaseIds = r.RoleUseCases
                                        .Select(rc => rc.UseCaseId)
                                        .OrderBy(x => x)
                                        .ToList()
                      })
                      .ToList();
        }
    }
}
