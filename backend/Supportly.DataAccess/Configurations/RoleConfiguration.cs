using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Supportly.DataAccess.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(255);

            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasData(
                new Role { Id = 1, Name = "admin", Description = "Administrator sistema" },
                new Role { Id = 2, Name = "agent", Description = "Agent podrške" },
                new Role { Id = 3, Name = "customer", Description = "Klijent / korisnik" }
            );
        }
    }
}
