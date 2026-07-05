using Application;
using Application.DTO;
using Application.DTO.Search;
using Application.Queries;
using Domain;
using Domain.Authorization;
using Supportly.DataAccess;
using System.Collections;
using System.Linq;

namespace Implementation.UseCases.Queries
{
    public class EfGetTicketQuery : EfUseCase, IGetTicketQuery
    {
        private readonly IApplicationUser _user;
        public EfGetTicketQuery(LabDbContext context, IApplicationUser user) : base(context)
        {
            _user = user;
        }

        public string Name => "Get one ticket details";

        public string Id => UseCaseIds.GetOneTicket;

        public IEnumerable<TicketListItemDTO> Execute(long request)
        {
            bool canSeeInternal = _user.AllowedUseCases.Contains(UseCaseIds.ViewInternalComments);
            bool canViewAny = _user.AllowedUseCases.Contains(UseCaseIds.GetTickets);

            var query = ctx.Tickets.Where(x => x.Id == request);
            if (!canViewAny)
                query = query.Where(x => x.RequesterId == _user.Id);

            return query.Select(x => new TicketListItemDTO{
                Id = x.Id,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
                Priority = x.Priority.Name,
                Status = x.Status.Name,
                IsClosed = x.Status.IsClosed,
                Subject = x.Subject,
                Description = x.Description,
                Category = x.Category.Name,
                Requester = x.Requester.FullName,
                Assignee = x.Assignee.FullName,
                TicketNumber = x.TicketNumber,
                Comments = x.Comments.Where(c => canSeeInternal || !c.IsInternal)
                                     .Select(t=> new TicketCommentsDTO {
                                                CreatedAt= t.CreatedAt,
                                                Author = t.Author.FullName,
                                                Body = t.Body,
                                                Id = t.Id,
                                                IsInternal = t.IsInternal,
                                                Attachments = t.Attachments.Select(a => new AttachmentDTO {
                                                                Id = a.Id,
                                                                FileName = a.FileName,
                                                                FilePath = a.FilePath,
                                                                MimeType = a.MimeType
                                                            }).ToList()
                                            }).ToList(),
                Attachments = x.Attachments.Where(a => a.CommentId == null).Select(a => new AttachmentDTO {
                                                Id = a.Id,
                                                FileName = a.FileName,
                                                FilePath = a.FilePath,
                                                MimeType = a.MimeType
                                            }).ToList(),
            }).ToList();
        }
    }
}
