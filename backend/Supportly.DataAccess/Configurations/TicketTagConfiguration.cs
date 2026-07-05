using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Supportly.DataAccess.Configurations
{
    public class TicketTagConfiguration : IEntityTypeConfiguration<TicketTag>
    {
        public void Configure(EntityTypeBuilder<TicketTag> builder)
        {
            builder.ToTable("TicketTags");
            builder.HasKey(x => new { x.TicketId, x.TagId });

            builder.HasOne(x => x.Ticket)
                   .WithMany(t => t.TicketTags)
                   .HasForeignKey(x => x.TicketId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_TicketTags_Tickets");

            builder.HasOne(x => x.Tag)
                   .WithMany(t => t.TicketTags)
                   .HasForeignKey(x => x.TagId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_TicketTags_Tags");
        }
    }
}
