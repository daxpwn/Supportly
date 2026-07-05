using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Supportly.DataAccess.Configurations
{
    public class TicketHistoryConfiguration : IEntityTypeConfiguration<TicketHistory>
    {
        public void Configure(EntityTypeBuilder<TicketHistory> builder)
        {
            builder.ToTable("TicketHistory");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FieldName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.OldValue).HasMaxLength(255);
            builder.Property(x => x.NewValue).HasMaxLength(255);
            builder.Property(x => x.ChangedAt).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");

            builder.HasIndex(x => x.TicketId).HasDatabaseName("IX_History_Ticket");

            builder.HasOne(x => x.Ticket)
                   .WithMany(t => t.History)
                   .HasForeignKey(x => x.TicketId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_History_Tickets");

            builder.HasOne(x => x.ChangedByUser)
                   .WithMany(u => u.HistoryChanges)
                   .HasForeignKey(x => x.ChangedBy)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_History_Users");
        }
    }
}
