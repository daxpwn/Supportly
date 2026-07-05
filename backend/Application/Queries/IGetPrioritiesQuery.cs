using Application.DTO;
using Application.DTO.Search;
using System.Collections.Generic;

namespace Application.Queries
{
    public interface IGetPrioritiesQuery : IQuery<PrioritySearch, IEnumerable<PriorityDTO>>
    {
    }
}
