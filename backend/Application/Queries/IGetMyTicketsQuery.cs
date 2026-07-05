using Application.DTO;
using Application.DTO.Search;

namespace Application.Queries
{
    public interface IGetMyTicketsQuery : IQuery<TicketSearch, PagedResponse<TicketListItemDTO>>
    {
    }
}
