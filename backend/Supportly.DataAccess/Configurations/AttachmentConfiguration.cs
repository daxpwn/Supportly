using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Supportly.DataAccess.Configurations
{
    public class AttachmentConfiguration : IEntityTypeConfiguration<Attachment>
    {
        public void Configure(EntityTypeBuilder<Attachment> builder)
        {
            builder.ToTable("Attachments");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FileName).HasMaxLength(255).IsRequired();
            builder.Property(x => x.FilePath).HasMaxLength(500).IsRequired();
            builder.Property(x => x.MimeType).HasMaxLength(100);
            builder.Property(x => x.CreatedAt).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");

            builder.HasOne(x => x.Ticket)
                   .WithMany(t => t.Attachments)
                   .HasForeignKey(x => x.TicketId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Attachments_Tickets");

            builder.HasOne(x => x.Comment)
                   .WithMany(c => c.Attachments)
                   .HasForeignKey(x => x.CommentId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Attachments_Comments");

            builder.HasOne(x => x.Uploader)
                   .WithMany(u => u.Attachments)
                   .HasForeignKey(x => x.UploadedBy)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Attachments_Users");
        }
    }
}
