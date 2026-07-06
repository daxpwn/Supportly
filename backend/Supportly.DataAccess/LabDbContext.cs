using Supportly.DataAccess.Configurations;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Supportly.DataAccess
{
    public class LabDbContext : DbContext
    {
        private string _connString;

        public LabDbContext(string connString)
        {
            _connString = connString;
        }

        public LabDbContext()
        {
            _connString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Supportly;TrustServerCertificate=true;Integrated Security=true";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connString).UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LabDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }

        // Auth infrastruktura
        public DbSet<User> Users { get; set; }
        public DbSet<RoleUseCase> RoleUseCases { get; set; }
        public DbSet<AuthToken> AuthTokens { get; set; }

        // Helpdesk domen
        public DbSet<Role> Roles { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Priority> Priorities { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketComment> TicketComments { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TicketTag> TicketTags { get; set; }
        public DbSet<TicketHistory> TicketHistory { get; set; }
        public DbSet<CannedResponse> CannedResponses { get; set; }

        // Audit log (svaki pokušaj izvršavanja use case-a)
        public DbSet<UseCaseLog> UseCaseLogs { get; set; }
    }
}
