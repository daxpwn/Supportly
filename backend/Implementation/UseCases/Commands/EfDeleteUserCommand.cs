using Application.Commands;
using Domain.Authorization;
using FluentValidation;
using FluentValidation.Results;
using Supportly.DataAccess;
using System.Linq;

namespace Implementation.UseCases.Commands
{
    public class EfDeleteUserCommand : EfUseCase, IDeleteUserCommand
    {
        public EfDeleteUserCommand(LabDbContext context) : base(context)
        {
        }

        public string Name => "Delete user (soft delete)";

        public string Id => UseCaseIds.DeleteUser;

        public void Execute(int data)
        {
            var user = ctx.Users.FirstOrDefault(u => u.Id == data);
            if (user == null)
                throw new ValidationException(new[]
                {
                    new ValidationFailure("Id", "User doesn't exist.")
                });

            // Soft-delete: ne brišemo red (čuva FK integritet ka tiketima/komentarima),
            // samo deaktiviramo korisnika.
            user.IsActive = false;
            user.UpdatedAt = System.DateTime.UtcNow;

            ctx.SaveChanges();
        }
    }
}
