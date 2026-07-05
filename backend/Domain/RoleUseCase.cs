namespace Domain
{
    // Dozvole po ROLI: koja rola sme koji use case (kompozitni ključ RoleId + UseCaseId).
    // Korisnik nasleđuje dozvole kroz svoju rolu (User.RoleId).
    public class RoleUseCase
    {
        public byte RoleId { get; set; }
        public string UseCaseId { get; set; } //npr. register, create-ticket, assign-ticket, get-users, etc.

        public virtual Role Role { get; set; }
    }
}
