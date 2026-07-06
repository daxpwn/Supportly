using Application.Commands;
using Application.DTO;
using Domain;
using Domain.Authorization;
using FluentValidation;
using FluentValidation.Results;
using Supportly.DataAccess;
using System.Linq;

namespace Implementation.UseCases.Commands
{
    public class EfAddRoleUseCaseCommand : EfUseCase, IAddRoleUseCaseCommand
    {
        public EfAddRoleUseCaseCommand(LabDbContext context) : base(context)
        {
        }

        public string Name => "Add use case to role";

        public string Id => UseCaseIds.ManageRoles;

        public void Execute(RoleUseCaseDTO data)
        {
            if (!ctx.Roles.Any(r => r.Id == data.RoleId))
                throw new ValidationException(new[]
                {
                    new ValidationFailure("RoleId", "Role doesn't exist.")
                });

            if (string.IsNullOrWhiteSpace(data.UseCaseId) || !UseCaseCatalog.All.Contains(data.UseCaseId))
                throw new ValidationException(new[]
                {
                    new ValidationFailure("UseCaseId", "Unknown use case.")
                });

            bool exists = ctx.RoleUseCases
                             .Any(rc => rc.RoleId == data.RoleId && rc.UseCaseId == data.UseCaseId);
            if (exists)
                return; // idempotentno — već dodeljeno

            ctx.RoleUseCases.Add(new RoleUseCase
            {
                RoleId = data.RoleId,
                UseCaseId = data.UseCaseId
            });
            ctx.SaveChanges();
        }
    }
}
