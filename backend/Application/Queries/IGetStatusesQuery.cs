using Application.DTO;
using Application.DTO.Search;
using System.Collections.Generic;

namespace Application.Queries
{
    public interface IGetStatusesQuery : IQuery<StatusSearch, IEnumerable<StatusDTO>>
    {
    }
}
