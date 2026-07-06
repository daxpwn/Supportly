using System;
using System.Collections.Generic;

namespace Domain.Authorization
{
    /// <summary>
    /// Izvor istine za seed tabele RoleUseCases: koje use case-ove ima koja rola.
    /// Seeder (/api/seed) dopunjava RoleUseCases iz ovog šablona (dodaje samo ono što fali).
    /// Novi use case: dodaj konstantu u UseCaseIds, upiši ga ovde u odgovarajuće role,
    /// pa pozovi /api/seed. Korisnik dozvole nasleđuje kroz svoju rolu (User.RoleId).
    /// </summary>
    public static class RoleUseCaseTemplate
    {
        public static IReadOnlyList<string> ForRole(string roleName)
        {
            switch (roleName)
            {
                case "admin":
                    return new[]
                    {
                        UseCaseIds.GetTickets, UseCaseIds.GetOneTicket,
                        UseCaseIds.CreateTicket, UseCaseIds.InsertTicket,
                        UseCaseIds.AssignTicket, UseCaseIds.ChangeTicketStatus,
                        UseCaseIds.ViewInternalComments, UseCaseIds.InsertComment, UseCaseIds.CommentAnyTicket,
                        UseCaseIds.UploadAttachment,
                        UseCaseIds.GetUsers, UseCaseIds.GetOneUser, UseCaseIds.UpdateUser, UseCaseIds.DeleteUser,
                        UseCaseIds.GetCategories, UseCaseIds.GetDepartments,
                        UseCaseIds.GetPriorities, UseCaseIds.GetStatuses,
                        UseCaseIds.GetUseCaseLogs, UseCaseIds.ManageRoles
                    };
                case "agent":
                    return new[]
                    {
                        UseCaseIds.GetTickets, UseCaseIds.GetOneTicket,
                        UseCaseIds.CreateTicket, UseCaseIds.InsertTicket,
                        UseCaseIds.AssignTicket, UseCaseIds.ChangeTicketStatus,
                        UseCaseIds.ViewInternalComments, UseCaseIds.InsertComment, UseCaseIds.CommentAnyTicket,
                        UseCaseIds.UploadAttachment,
                        UseCaseIds.GetCategories, UseCaseIds.GetDepartments,
                        UseCaseIds.GetPriorities, UseCaseIds.GetStatuses
                    };
                case "customer":
                    return new[]
                    {
                        UseCaseIds.CreateTicket, UseCaseIds.InsertTicket, UseCaseIds.InsertComment,
                        UseCaseIds.UploadAttachment,
                        UseCaseIds.GetMyTickets, UseCaseIds.GetOneTicket,
                        UseCaseIds.GetCategories, UseCaseIds.GetDepartments,
                        UseCaseIds.GetPriorities, UseCaseIds.GetStatuses
                    };
                default:
                    return Array.Empty<string>();
            }
        }
    }
}
