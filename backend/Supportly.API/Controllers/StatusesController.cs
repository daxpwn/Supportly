using Application.DTO.Search;
using Application.Queries;
using Implementation.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Supportly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusesController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public StatusesController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        // GET: api/statuses
        [HttpGet]
        [Authorize]
        public IActionResult Get([FromServices] IGetStatusesQuery query,
                                 [FromQuery] StatusSearch search)
            => Ok(_handler.ExecuteQuery(query, search));
    }
}
