using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Supportly.DataAccess.Configurations
{
    public class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            builder.ToTable("Statuses");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).HasMaxLength(30).IsRequired();
            builder.Property(x => x.IsClosed).HasDefaultValue(false);

            builder.HasIndex(x => x.Name).IsUnique();

            builder.HasData(
                new Status { Id = 1, Name = "Otvoren", IsClosed = false },
                new Status { Id = 2, Name = "U obradi", IsClosed = false },
                new Status { Id = 3, Name = "Čeka klijenta", IsClosed = false },
                new Status { Id = 4, Name = "Rešen", IsClosed = true },
                new Status { Id = 5, Name = "Zatvoren", IsClosed = true }
            );
        }
    }
}
