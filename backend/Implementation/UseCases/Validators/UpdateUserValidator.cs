using Application.DTO;
using FluentValidation;
using Supportly.DataAccess;
using System.Linq;

namespace Implementation.UseCases.Validators
{
    public class UpdateUserValidator : AbstractValidator<UserUpdateDTO>
    {
        private const string required = "Field is required.";
        public UpdateUserValidator(LabDbContext ctx)
        {
            this.ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Id)
                .Must(id => ctx.Users.Any(u => u.Id == id)).WithMessage("User doesn't exist.");

            RuleFor(x => x.FullName).NotEmpty().WithMessage(required).MinimumLength(3).MaximumLength(150);

            RuleFor(x => x.Email).NotEmpty().WithMessage(required).EmailAddress()
                .Must((dto, email) => !ctx.Users.Any(u => u.Email == email && u.Id != dto.Id))
                .WithMessage("Email is in use.");

            RuleFor(x => x.RoleId)
                .Must(id => ctx.Roles.Any(r => r.Id == id)).WithMessage("Role doesn't exist.");

            RuleFor(x => x.DepartmentId)
                .Must(id => ctx.Departments.Any(d => d.Id == id.Value)).WithMessage("Department doesn't exist.")
                .When(x => x.DepartmentId.HasValue);
        }
    }
}
