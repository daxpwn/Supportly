using Application.DTO.Search;
using Application.Queries;
using Implementation.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Supportly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public AuditLogController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        // GET: api/auditlog?userId=&username=&useCaseName=&from=&to=&page=&perPage=
        // Pretraga audit loga po korisniku, nazivu use case-a i opsegu datuma.
        [HttpGet]
        [Authorize]
        public IActionResult Get([FromServices] IGetUseCaseLogsQuery query,
                                 [FromQuery] UseCaseLogSearch search)
            => Ok(_handler.ExecuteQuery(query, search));
    }
}
