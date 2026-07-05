using Application.DTO;
using FluentValidation;
using Supportly.DataAccess;
using System.Linq;

namespace Implementation.UseCases.Validators
{
    public class InsertTicketValidator : AbstractValidator<TicketInsertDTO>
    {
        private const string required = "Field is required.";
        public InsertTicketValidator(LabDbContext ctx)
        {
            this.RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Subject).NotEmpty().WithMessage(required).MinimumLength(3).MaximumLength(255);
            RuleFor(x => x.Description).NotEmpty().WithMessage(required).MinimumLength(3);

            RuleFor(x => x.PriorityId)
                .Must(id => ctx.Priorities.Any(p => p.Id == id)).WithMessage("Priority doesn't exist.");

            RuleFor(x => x.DepartmentId)
                .Must(id => ctx.Departments.Any(d => d.Id == id.Value)).WithMessage("Department doesn't exist.")
                .When(x => x.DepartmentId.HasValue);

            RuleFor(x => x.CategoryId)
                .Must(id => ctx.Categories.Any(c => c.Id == id.Value)).WithMessage("Category doesn't exist.")
                .When(x => x.CategoryId.HasValue);
        }
    }
}
