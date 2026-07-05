using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Supportly.DataAccess.Configurations
{
    public class PriorityConfiguration : IEntityTypeConfiguration<Priority>
    {
        public void Configure(EntityTypeBuilder<Priority> builder)
        {
            builder.ToTable("Priorities", t =>
                t.HasCheckConstraint("CK_Priorities_Level", "[Level] BETWEEN 1 AND 10"));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(30).IsRequired();
            builder.Property(x => x.Level).IsRequired();

            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasData(
                new Priority { Id = 1, Name = "Nizak", Level = 1 },
                new Priority { Id = 2, Name = "Srednji", Level = 2 },
                new Priority { Id = 3, Name = "Visok", Level = 3 },
                new Priority { Id = 4, Name = "Hitan", Level = 4 }
            );
        }
    }
}
