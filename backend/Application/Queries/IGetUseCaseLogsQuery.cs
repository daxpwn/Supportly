using Application.DTO;
using Application.DTO.Search;

namespace Application.Queries
{
    public interface IGetUseCaseLogsQuery : IQuery<UseCaseLogSearch, PagedResponse<UseCaseLogDTO>>
    {
    }
}
