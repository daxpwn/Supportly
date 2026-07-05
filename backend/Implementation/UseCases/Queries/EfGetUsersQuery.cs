using Application.DTO;
using Application.DTO.Search;
using Application.Queries;
using Domain.Authorization;
using Supportly.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace Implementation.UseCases.Queries
{
    public class EfGetUsersQuery : EfUseCase, IGetUsersQuery
    {
        public EfGetUsersQuery(LabDbContext context) : base(context)
        {
        }

        public string Name => "Get users";

        public string Id => UseCaseIds.GetUsers;

        public IEnumerable<UserDTO> Execute(UserSearch request)
        {
            return ctx.Users.Select(x => new UserDTO {
                                                     Id = x.Id,
                                                     CreatedAt = x.CreatedAt,
                                                     IsActive = x.IsActive,
                                                     Email  = x.Email,
                                                     FullName = x.FullName,
                                                     Phone= x.Phone,
                                                     Role = x.Role.Name
                                                     }).ToList();
        }
    }
}
