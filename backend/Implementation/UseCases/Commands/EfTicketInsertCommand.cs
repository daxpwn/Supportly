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
    public class EfTicketInsertCommand : EfUseCase, ITicketInsertCommand
    {
        private readonly InsertTicketValidator _validator;
        private readonly IApplicationUser _user;

        public EfTicketInsertCommand(LabDbContext context, InsertTicketValidator validator, IApplicationUser user) : base(context)
        {
            _user = user;
            _validator = validator;
        }

        public string Name => "Insert ticket by customer";

        public string Id => UseCaseIds.InsertTicket;

        public void Execute(TicketInsertDTO data)
        {
            _validator.ValidateAndThrow(data);

            var now = DateTime.UtcNow;

            var ticket = new Ticket
            {
                TicketNumber = Guid.NewGuid().ToString(),
                Subject = data.Subject,
                Description = data.Description,
                RequesterId = _user.Id,
                DepartmentId = (short?)data.DepartmentId,
                CategoryId = (short?)data.CategoryId,
                PriorityId = (byte)data.PriorityId,
                StatusId = 1,
                DueAt = data.DueAt,
                CreatedAt = now,
                UpdatedAt = now
            };

            ctx.Tickets.Add(ticket);
            ctx.SaveChanges();

            // Vrati id/broj kreiranog tiketa kroz DTO (koristi ga kontroler u odgovoru).
            data.Id = ticket.Id;
            data.TicketNumber = ticket.TicketNumber;

            // Napomena: data.AttachmentIds (povezivanje priloga) nije obrađeno ovde — zaseban korak.
        }
    }
}
