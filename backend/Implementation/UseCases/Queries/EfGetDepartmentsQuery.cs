using Application.DTO;
using Application.DTO.Search;
using Application.Queries;
using Domain.Authorization;
using Supportly.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace Implementation.UseCases.Queries
{
    public class EfGetDepartmentsQuery : EfUseCase, IGetDepartmentsQuery
    {
        public EfGetDepartmentsQuery(LabDbContext context) : base(context)
        {
        }

        public string Name => "Vraca sva odeljenja";

        public string Id => UseCaseIds.GetDepartments;

        public IEnumerable<DepartmentDTO> Execute(DepartmentSearch search)
        {
            return ctx.Departments
                      .Select(d => new DepartmentDTO
                      {
                          Id = d.Id,
                          Name = d.Name,
                          Email = d.Email
                      })
                      .ToList();
        }
    }
}
