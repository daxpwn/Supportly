using Application.DTO;
using Application.DTO.Search;
using System.Collections.Generic;

namespace Application.Queries
{
    public interface IGetDepartmentsQuery : IQuery<DepartmentSearch, IEnumerable<DepartmentDTO>>
    {
    }
}
