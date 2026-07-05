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
    public class EfUploadAttachmentCommand : EfUseCase, IUploadAttachmentCommand
    {
        private readonly UploadAttachmentValidator _validator;
        private readonly IApplicationUser _user;
        private readonly IFileStorage _storage;

        public EfUploadAttachmentCommand(LabDbContext context,
                                         UploadAttachmentValidator validator,
                                         IApplicationUser user,
                                         IFileStorage storage) : base(context)
        {
            _validator = validator;
            _user = user;
            _storage = storage;
        }

        public string Name => "Upload attachment";

        public string Id => UseCaseIds.UploadAttachment;

        public void Execute(AttachmentUploadDTO data)
        {
            _validator.ValidateAndThrow(data);

            // 1) snimi fajl na disk -> dobij relativnu putanju
            var path = _storage.Save(data.Content, data.FileName);

            // 2) upiši metapodatak u bazu
            var attachment = new Attachment
            {
                TicketId = data.TicketId,
                CommentId = data.CommentId,
                FileName = data.FileName,
                FilePath = path,
                MimeType = data.ContentType,
                FileSize = data.Content.Length,
                UploadedBy = _user.Id,
                CreatedAt = DateTime.UtcNow
            };

            ctx.Attachments.Add(attachment);
            ctx.SaveChanges();
        }
    }
}
