using System;
using System.Linq;
using Application.Commands;
using Application.DTO;
using Domain.Authorization;
using Supportly.DataAccess;
using FluentValidation;
using FluentValidation.Results;

namespace Implementation.UseCases.Commands
{
    public class EfChangeTicketStatus : EfUseCase, IChangeTicketStatusCommand
    {
        public EfChangeTicketStatus(LabDbContext context) : base(context)
        {
        }

        public string Name => "Change ticket status";

        public string Id => UseCaseIds.ChangeTicketStatus;

        public void Execute(ChangeTicketStatusDTO data)
        {
            var ticket = ctx.Tickets.FirstOrDefault(t => t.Id == data.TicketId);
            if (ticket == null)
                throw new ValidationException(new[]
                {
                    new ValidationFailure("TicketId", "Ticket doesn't exist.")
                });

            if (!ctx.Statuses.Any(s => s.Id == data.StatusId))
                throw new ValidationException(new[]
                {
                    new ValidationFailure("StatusId", "Status ID is not correct.")
                });

            ticket.StatusId = (byte)data.StatusId;
            ticket.UpdatedAt = DateTime.UtcNow;

            ctx.SaveChanges();
        }
    }
}
