using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Supportly.DataAccess.Configurations
{
    public class CannedResponseConfiguration : IEntityTypeConfiguration<CannedResponse>
    {
        public void Configure(EntityTypeBuilder<CannedResponse> builder)
        {
            builder.ToTable("CannedResponses");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Body).IsRequired(); // NVARCHAR(MAX)

            builder.HasOne(x => x.Creator)
                   .WithMany(u => u.CannedResponses)
                   .HasForeignKey(x => x.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Canned_Users");
        }
    }
}
