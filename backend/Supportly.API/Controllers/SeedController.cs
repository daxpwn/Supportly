using Supportly.DataAccess;
using Supportly.DataAccess.Seeder;
using Microsoft.AspNetCore.Mvc;

namespace Supportly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedController : ControllerBase
    {
        private readonly LabDbContext _ctx;

        public SeedController(LabDbContext ctx)
        {
            _ctx = ctx;
        }

        // GET /api/seed
        // Javno dostupno — popunjava bazu početnim podacima (idempotentno).
        [HttpGet]
        public IActionResult Seed()
        {
            bool seeded = DatabaseSeeder.Seed(_ctx);

            return seeded
                ? Ok(new { message = "Baza je uspešno zasejana početnim podacima." })
                : Ok(new { message = "Podaci već postoje — seedovanje preskočeno." });
        }
    }
}
