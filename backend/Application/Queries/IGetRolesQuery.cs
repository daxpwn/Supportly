using Application.DTO;
using System.Collections.Generic;

namespace Application.Queries
{
    public interface IGetRolesQuery : IQuery<object, IEnumerable<RoleWithUseCasesDTO>>
    {
    }
}
