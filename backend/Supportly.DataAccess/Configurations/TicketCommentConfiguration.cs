using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Supportly.DataAccess.Configurations
{
    public class TicketCommentConfiguration : IEntityTypeConfiguration<TicketComment>
    {
        public void Configure(EntityTypeBuilder<TicketComment> builder)
        {
            builder.ToTable("TicketComments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Body).IsRequired(); // NVARCHAR(MAX)
            builder.Property(x => x.IsInternal).HasDefaultValue(false);
            builder.Property(x => x.CreatedAt).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");

            builder.HasIndex(x => x.TicketId).HasDatabaseName("IX_Comments_Ticket");

            builder.HasOne(x => x.Ticket)
                   .WithMany(t => t.Comments)
                   .HasForeignKey(x => x.TicketId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Comments_Tickets");

            builder.HasOne(x => x.Author)
                   .WithMany(u => u.Comments)
                   .HasForeignKey(x => x.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Comments_Users");
        }
    }
}
