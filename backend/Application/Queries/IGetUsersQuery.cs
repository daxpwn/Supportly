using Application.DTO;
using Application.DTO.Search;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Queries
{
    public interface IGetUsersQuery : IQuery<UserSearch, IEnumerable<UserDTO>>
    {
    }
}
