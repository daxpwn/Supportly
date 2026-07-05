using Application.DTO;
using Application.Queries;
using Domain.Authorization;
using Supportly.DataAccess;
using System.Linq;

namespace Implementation.UseCases.Queries
{
    public class EfGetUserQuery : EfUseCase, IGetUserQuery
    {
        public EfGetUserQuery(LabDbContext context) : base(context)
        {
        }

        public string Name => "Get one user";

        public string Id => UseCaseIds.GetOneUser;

        public UserDetailsDTO Execute(int request)
        {
            return ctx.Users
                      .Where(u => u.Id == request)
                      .Select(u => new UserDetailsDTO
                      {
                          Id = u.Id,
                          FullName = u.FullName,
                          Email = u.Email,
                          Phone = u.Phone,
                          RoleId = u.RoleId,
                          Role = u.Role.Name,
                          DepartmentId = u.DepartmentId,
                          Department = u.Department.Name,
                          IsActive = u.IsActive,
                          CreatedAt = u.CreatedAt,
                          UpdatedAt = u.UpdatedAt
                      })
                      .FirstOrDefault();
        }
    }
}
