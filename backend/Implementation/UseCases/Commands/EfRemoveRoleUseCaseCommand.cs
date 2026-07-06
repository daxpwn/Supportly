using Application.Commands;
using Application.DTO;
using Domain.Authorization;
using FluentValidation;
using FluentValidation.Results;
using Supportly.DataAccess;
using System.Linq;

namespace Implementation.UseCases.Commands
{
    public class EfRemoveRoleUseCaseCommand : EfUseCase, IRemoveRoleUseCaseCommand
    {
        public EfRemoveRoleUseCaseCommand(LabDbContext context) : base(context)
        {
        }

        public string Name => "Remove use case from role";

        public string Id => UseCaseIds.ManageRoles;

        public void Execute(RoleUseCaseDTO data)
        {
            var entity = ctx.RoleUseCases
                            .FirstOrDefault(rc => rc.RoleId == data.RoleId && rc.UseCaseId == data.UseCaseId);

            if (entity == null)
                throw new ValidationException(new[]
                {
                    new ValidationFailure("UseCaseId", "Assignment doesn't exist.")
                });

            ctx.RoleUseCases.Remove(entity);
            ctx.SaveChanges();
        }
    }
}
