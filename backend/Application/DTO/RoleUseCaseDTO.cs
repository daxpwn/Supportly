namespace Application.DTO
{
    // Dodela / uklanjanje jednog use case-a jednoj roli.
    public class RoleUseCaseDTO
    {
        public byte RoleId { get; set; }
        public string UseCaseId { get; set; }
    }
}
