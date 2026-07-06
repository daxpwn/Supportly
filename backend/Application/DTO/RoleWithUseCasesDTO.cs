using System.Collections.Generic;

namespace Application.DTO
{
    public class RoleWithUseCasesDTO
    {
        public byte Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<string> UseCaseIds { get; set; } = new List<string>();
    }
}
