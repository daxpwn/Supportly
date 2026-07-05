using Application;
using Application.DTO;
using Domain.Authorization;
using FluentValidation;
using Supportly.DataAccess;
using System.Linq;

namespace Implementation.UseCases.Validators
{
    public class InsertCommentValidator : AbstractValidator<TicketCommentInsertDTO>
    {
        private const string required = "Field is required.";
        public InsertCommentValidator(LabDbContext ctx, IApplicationUser user)
        {
            this.ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Body).NotEmpty().WithMessage(required).MinimumLength(3);

            RuleFor(x => x.TicketId)
                .Must(id => ctx.Tickets.Any(t => t.Id == id)).WithMessage("Ticket doesn't exist.");

            RuleFor(x => x.TicketId)
                .Must(id => ctx.Tickets.Any(t => t.Id == id && t.RequesterId == user.Id))
                .WithMessage("You can only comment on your own tickets.")
                .When(x => !user.AllowedUseCases.Contains(UseCaseIds.CommentAnyTicket));

            RuleFor(x => x.TicketId)
                .Must(id => !ctx.Tickets.Any(t => t.Id == id && t.Status.IsClosed))
                .WithMessage("Tiket is closed.")
                .When(x => !user.AllowedUseCases.Contains(UseCaseIds.CommentAnyTicket));

            RuleFor(x => x.IsInternal)
                .Equal(false)
                .WithMessage("Only staff can post internal comments.")
                .When(x => !user.AllowedUseCases.Contains(UseCaseIds.ViewInternalComments));
        }
    }
}