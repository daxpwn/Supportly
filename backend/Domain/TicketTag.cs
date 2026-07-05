namespace Domain
{
    // Veza više-prema-više između tiketa i tagova (kompozitni ključ TicketId + TagId)
    public class TicketTag
    {
        public long TicketId { get; set; }
        public short TagId { get; set; }

        public virtual Ticket Ticket { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
