using Application;
using Application.DTO;
using Domain.Authorization;
using FluentValidation;
using Supportly.DataAccess;
using System.IO;
using System.Linq;

namespace Implementation.UseCases.Validators
{
    public class UploadAttachmentValidator : AbstractValidator<AttachmentUploadDTO>
    {
        private const long MaxBytes = 10 * 1024 * 1024; // 10 MB
        private static readonly string[] AllowedExtensions =
            { ".pdf", ".png", ".jpg", ".jpeg", ".gif", ".txt", ".doc", ".docx", ".xls", ".xlsx", ".zip" };

        public UploadAttachmentValidator(LabDbContext ctx, IApplicationUser user)
        {
            this.ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Content)
                .Must(c => c != null && c.Length > 0).WithMessage("File is empty.")
                .Must(c => c.Length <= MaxBytes).WithMessage("File is too large (max 10 MB).");

            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage("File name is required.")
                .Must(name => AllowedExtensions.Contains(Path.GetExtension(name).ToLowerInvariant()))
                .WithMessage("File type is not allowed.");

            RuleFor(x => x.TicketId)
                .Must(id => ctx.Tickets.Any(t => t.Id == id)).WithMessage("Ticket doesn't exist.");

            RuleFor(x => x.TicketId)
                .Must(id => ctx.Tickets.Any(t => t.Id == id && t.RequesterId == user.Id))
                .WithMessage("You can only attach files to your own tickets.")
                .When(x => !user.AllowedUseCases.Contains(UseCaseIds.GetTickets));

            RuleFor(x => x.CommentId)
                .Must((dto, commentId) => ctx.TicketComments.Any(c => c.Id == commentId.Value && c.TicketId == dto.TicketId))
                .WithMessage("Comment doesn't exist on this ticket.")
                .When(x => x.CommentId.HasValue);

            RuleFor(x => x.TicketId)
            .Must(id => !ctx.Tickets.Any(t => t.Id == id && t.Status.IsClosed))
            .WithMessage("Tiket is closed.")
            .When(x => !user.AllowedUseCases.Contains(UseCaseIds.CommentAnyTicket));
        }
    }
}
