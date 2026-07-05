using System.Collections.Generic;

namespace Domain
{
    public class Role
    {
        public byte Id { get; set; }            // TINYINT
        public string Name { get; set; }
        public string Description { get; set; } // NULL dozvoljeno

        public virtual HashSet<User> Users { get; set; } = new HashSet<User>();

        // Dozvole ove role (use case-ovi koje rola sme da izvrši)
        public virtual HashSet<RoleUseCase> RoleUseCases { get; set; } = new HashSet<RoleUseCase>();
    }
}
