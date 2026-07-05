namespace Domain.Authorization
{
    /// <summary>
    /// Stabilni identifikatori use case-ova — ista vrednost kao `Id` u komandama/upitima
    /// i kao `RoleUseCase.UseCaseId` u bazi. Nove use case-ove dodaj ovde da se izbegnu
    /// magični stringovi. (Naziv "UseCaseIds" da ne kolidira sa namespace-om Implementation.UseCases.)
    /// </summary>
    public static class UseCaseIds
    {
        public const string Register = "register";

        public const string GetTickets = "get-tickets";
        public const string GetMyTickets = "get-my-tickets";
        public const string GetOneTicket = "get-one-ticket";
        public const string ViewInternalComments = "view-internal-comments";
        public const string InsertComment = "insert-comment";
        public const string UploadAttachment = "upload-attachment";

        public const string CommentAnyTicket = "comment-any-ticket";

        public const string CreateTicket = "create-ticket";
        public const string AssignTicket = "assign-ticket";
        public const string ChangeTicketStatus = "change-ticket-status";
        public const string GetUsers = "get-users";
        public const string GetOneUser = "get-one-user";
        public const string UpdateUser = "update-user";
        public const string DeleteUser = "delete-user";
        public const string GetCategories = "get-categories";
        public const string GetDepartments = "get-departments";
        public const string GetPriorities = "get-priorities";
        public const string GetStatuses = "get-statuses";
        public const string InsertTicket = "insert-ticket";
    }
}
