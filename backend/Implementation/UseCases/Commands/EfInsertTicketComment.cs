using Application;
using Application.Commands;
using Application.DTO;
using Domain;
using Domain.Authorization;
using FluentValidation;
using Implementation.UseCases.Validators;
using Supportly.DataAccess;
using System;

namespace Implementation.UseCases.Commands
{
    public class EfInsertTicketComment : EfUseCase, ITicketInsertCommentCommand
    {
        private readonly InsertCommentValidator _validator;
        private readonly IApplicationUser _user;

        public EfInsertTicketComment(LabDbContext context, InsertCommentValidator validator, IApplicationUser user) : base(context)
        {
            _validator = validator;
            _user = user;
        }

        public string Name => "Insert comment by customer";

        public string Id => UseCaseIds.InsertComment;

        public void Execute(TicketCommentInsertDTO data)
        {
            _validator.ValidateAndThrow(data);

            var now = DateTime.UtcNow;

            var comment = new TicketComment
            {
                TicketId = data.TicketId,
                AuthorId = _user.Id,
                Body = data.Body,
                IsInternal = data.IsInternal,
                CreatedAt = now,
            };

            ctx.TicketComments.Add(comment);
            ctx.SaveChanges();

            // Vrati id kreiranog komentara kroz DTO (koristi ga kontroler u odgovoru).
            data.Id = comment.Id;
        }
    }
}
