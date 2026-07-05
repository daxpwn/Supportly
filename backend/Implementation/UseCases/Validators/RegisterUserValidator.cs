using Application.DTO;
using Supportly.DataAccess;
using FluentValidation;
using System.Linq;

namespace Implementation.UseCases.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserDTO>
    {
        private const string required = "Field is required.";
        public RegisterUserValidator(LabDbContext ctx)
        {

            this.RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.FullName).NotEmpty().WithMessage(required).MinimumLength(3);

            RuleFor(x => x.Email).NotEmpty().WithMessage(required)
                                 .EmailAddress()
                                 .Must(x => !ctx.Users.Any(u => u.Email == x)).WithMessage("Email is in use.");

            RuleFor(x => x.Password).NotEmpty().WithMessage(required).Matches("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\\W_]).{8,}$").WithMessage("Password must be at least 8 characters long and contain at least one uppercase letter, one lowercase letter, one digit, and one special character.");
        }
    }
}
