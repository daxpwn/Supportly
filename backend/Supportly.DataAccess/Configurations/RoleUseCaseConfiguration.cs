using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Supportly.DataAccess.Configurations
{
    public class RoleUseCaseConfiguration : IEntityTypeConfiguration<RoleUseCase>
    {
        public void Configure(EntityTypeBuilder<RoleUseCase> builder)
        {
            builder.ToTable("RoleUseCases");
            builder.HasKey(x => new { x.RoleId, x.UseCaseId });

            builder.Property(x => x.UseCaseId).HasMaxLength(50).IsRequired();

            builder.HasOne(x => x.Role)
                   .WithMany(r => r.RoleUseCases)
                   .HasForeignKey(x => x.RoleId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_RoleUseCases_Roles");
        }
    }
}
