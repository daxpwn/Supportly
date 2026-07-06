using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Supportly.DataAccess.Configurations
{
    public class UseCaseLogConfiguration : IEntityTypeConfiguration<UseCaseLog>
    {
        public void Configure(EntityTypeBuilder<UseCaseLog> builder)
        {
            builder.ToTable("UseCaseLogs");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Username).HasMaxLength(200).IsRequired();
            builder.Property(x => x.UseCaseId).HasMaxLength(100).IsRequired();
            builder.Property(x => x.UseCaseName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Payload).HasMaxLength(2000);
            builder.Property(x => x.ExecutedAt).HasColumnType("datetime2(3)");

            // Indeksi za pretragu (korisnik, use case, datum)
            builder.HasIndex(x => x.ExecutedAt).HasDatabaseName("IX_UseCaseLogs_ExecutedAt");
            builder.HasIndex(x => x.UserId).HasDatabaseName("IX_UseCaseLogs_User");
            builder.HasIndex(x => x.UseCaseId).HasDatabaseName("IX_UseCaseLogs_UseCase");
        }
    }
}
