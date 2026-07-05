using Application.DTO;

namespace Application.Queries
{
    public interface IGetUserQuery : IQuery<int, UserDetailsDTO>
    {
    }
}
