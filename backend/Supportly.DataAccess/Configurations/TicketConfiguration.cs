using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Supportly.DataAccess.Configurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.ToTable("Tickets", t => t.HasTrigger("trg_Tickets_UpdatedAt"));
            builder.HasKey(x => x.Id);

            builder.Property(x => x.TicketNumber).HasMaxLength(36).IsUnicode(false).IsRequired(); // GUID -> varchar(36)
            builder.Property(x => x.Subject).HasMaxLength(255).IsRequired();
            builder.Property(x => x.Description).IsRequired(); // NVARCHAR(MAX)

            builder.Property(x => x.DueAt).HasColumnType("datetime2(0)");
            builder.Property(x => x.FirstResponseAt).HasColumnType("datetime2(0)");
            builder.Property(x => x.ResolvedAt).HasColumnType("datetime2(0)");
            builder.Property(x => x.ClosedAt).HasColumnType("datetime2(0)");
            builder.Property(x => x.CreatedAt).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");
            builder.Property(x => x.UpdatedAt).HasColumnType("datetime2(0)").HasDefaultValueSql("SYSDATETIME()");

            builder.HasIndex(x => x.TicketNumber).IsUnique();
            builder.HasIndex(x => x.StatusId).HasDatabaseName("IX_Tickets_Status");
            builder.HasIndex(x => x.AssigneeId).HasDatabaseName("IX_Tickets_Assignee");
            builder.HasIndex(x => x.RequesterId).HasDatabaseName("IX_Tickets_Requester");
            builder.HasIndex(x => x.CreatedAt).HasDatabaseName("IX_Tickets_CreatedAt");

            builder.HasOne(x => x.Requester)
                   .WithMany(u => u.RequestedTickets)
                   .HasForeignKey(x => x.RequesterId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Tickets_Requester");

            builder.HasOne(x => x.Assignee)
                   .WithMany(u => u.AssignedTickets)
                   .HasForeignKey(x => x.AssigneeId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Tickets_Assignee");

            builder.HasOne(x => x.Department)
                   .WithMany(d => d.Tickets)
                   .HasForeignKey(x => x.DepartmentId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Tickets_Departments");

            builder.HasOne(x => x.Category)
                   .WithMany(c => c.Tickets)
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Tickets_Categories");

            builder.HasOne(x => x.Priority)
                   .WithMany(p => p.Tickets)
                   .HasForeignKey(x => x.PriorityId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Tickets_Priorities");

            builder.HasOne(x => x.Status)
                   .WithMany(s => s.Tickets)
                   .HasForeignKey(x => x.StatusId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Tickets_Statuses");
        }
    }
}
