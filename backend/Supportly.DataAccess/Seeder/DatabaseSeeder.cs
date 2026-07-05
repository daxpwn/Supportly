using Domain;
using Domain.Authorization;
using System;
using System.Linq;

namespace Supportly.DataAccess.Seeder
{
    /// <summary>
    /// Inicijalni seeder. Šifarnici (Roles, Priorities, Statuses) već dolaze kroz
    /// migraciju (HasData), pa ovde dodajemo odeljenja, kategorije, korisnike i par demo tiketa.
    /// Idempotentno: ako korisnici već postoje, ne radi ništa.
    /// </summary>
    public static class DatabaseSeeder
    {
        /// <returns>true ako su podaci ubačeni; false ako su već postojali (preskočeno).</returns>
        public static bool Seed(LabDbContext ctx)
        {
            // Dozvole po roli (RoleUseCases) — dopunjava ono što fali, radi i kad podaci već postoje.
            EnsureRoleUseCases(ctx);

            if (ctx.Users.Any())
                return false; // osnovni podaci već postoje

            var now = DateTime.UtcNow;

            // --- Odeljenja ---
            var itDept = new Department { Name = "IT podrška", Email = "it@supportly.rs", CreatedAt = now };
            var csDept = new Department { Name = "Korisnička podrška", Email = "podrska@supportly.rs", CreatedAt = now };
            ctx.Departments.AddRange(itDept, csDept);
            ctx.SaveChanges();

            // --- Kategorije ---
            var hardver = new Category { Name = "Hardver", DepartmentId = itDept.Id };
            var softver = new Category { Name = "Softver", DepartmentId = itDept.Id };
            var nalog = new Category { Name = "Nalog i pristup", DepartmentId = csDept.Id };
            ctx.Categories.AddRange(hardver, softver, nalog);
            ctx.SaveChanges();

            // --- Korisnici (role iz seed migracije: admin / agent / customer) ---
            byte adminRole = ctx.Roles.First(r => r.Name == "admin").Id;
            byte agentRole = ctx.Roles.First(r => r.Name == "agent").Id;
            byte customerRole = ctx.Roles.First(r => r.Name == "customer").Id;

            var admin = new User
            {
                FullName = "Admin Supportly",
                Email = "admin@supportly.rs",
                PasswordHash = Hash("Admin123!"),
                RoleId = adminRole,
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            };
            var agent = new User
            {
                FullName = "Marko Agent",
                Email = "agent@supportly.rs",
                PasswordHash = Hash("Agent123!"),
                RoleId = agentRole,
                DepartmentId = itDept.Id,
                Phone = "0601111111",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            };
            var klijent = new User
            {
                FullName = "Pera Perić",
                Email = "klijent@supportly.rs",
                PasswordHash = Hash("Klijent123!"),
                RoleId = customerRole,
                Phone = "0602222222",
                IsActive = true,
                CreatedAt = now,
                UpdatedAt = now
            };
            ctx.Users.AddRange(admin, agent, klijent);
            ctx.SaveChanges();

            // --- Demo tiketi (status/prioritet iz seed migracije) ---
            byte otvoren = ctx.Statuses.First(s => s.Name == "Otvoren").Id;
            byte uObradi = ctx.Statuses.First(s => s.Name == "U obradi").Id;
            byte visok = ctx.Priorities.First(p => p.Name == "Visok").Id;
            byte srednji = ctx.Priorities.First(p => p.Name == "Srednji").Id;

            var t1 = new Ticket
            {
                TicketNumber = "HD-2026-000001",
                Subject = "Ne radi štampač",
                Description = "Štampač u kancelariji 12 ne reaguje na komande.",
                RequesterId = klijent.Id,
                AssigneeId = agent.Id,
                DepartmentId = itDept.Id,
                CategoryId = hardver.Id,
                PriorityId = visok,
                StatusId = uObradi,
                CreatedAt = now,
                UpdatedAt = now
            };
            var t2 = new Ticket
            {
                TicketNumber = "HD-2026-000002",
                Subject = "Zaboravljena lozinka",
                Description = "Ne mogu da se prijavim na svoj nalog.",
                RequesterId = klijent.Id,
                DepartmentId = csDept.Id,
                CategoryId = nalog.Id,
                PriorityId = srednji,
                StatusId = otvoren,
                CreatedAt = now,
                UpdatedAt = now
            };
            ctx.Tickets.AddRange(t1, t2);
            ctx.SaveChanges();

            return true;
        }

        // Popuni RoleUseCases za svaku rolu iz RoleUseCaseTemplate; dodaje samo redove koji fale.
        private static void EnsureRoleUseCases(LabDbContext ctx)
        {
            bool changed = false;

            foreach (var role in ctx.Roles.ToList())
            {
                var wanted = RoleUseCaseTemplate.ForRole(role.Name);
                var existing = ctx.RoleUseCases
                                  .Where(rc => rc.RoleId == role.Id)
                                  .ToList();

                // dodaj ono što fali
                foreach (var useCaseId in wanted)
                {
                    if (!existing.Any(rc => rc.UseCaseId == useCaseId))
                    {
                        ctx.RoleUseCases.Add(new RoleUseCase { RoleId = role.Id, UseCaseId = useCaseId });
                        changed = true;
                    }
                }

                // ukloni višak (dozvola koja više nije u šablonu za ovu rolu)
                foreach (var rc in existing)
                {
                    if (!wanted.Contains(rc.UseCaseId))
                    {
                        ctx.RoleUseCases.Remove(rc);
                        changed = true;
                    }
                }
            }

            if (changed)
                ctx.SaveChanges();
        }

        private static string Hash(string password) => BCrypt.Net.BCrypt.HashPassword(password);
    }
}
