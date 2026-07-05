using Application.DTO;

namespace Application.Commands
{
    public interface IUpdateUserCommand : ICommand<UserUpdateDTO>
    {
    }
}
