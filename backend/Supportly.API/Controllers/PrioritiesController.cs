using Application.DTO.Search;
using Application.Queries;
using Implementation.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Supportly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrioritiesController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public PrioritiesController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        // GET: api/priorities
        [HttpGet]
        [Authorize]
        public IActionResult Get([FromServices] IGetPrioritiesQuery query,
                                 [FromQuery] PrioritySearch search)
            => Ok(_handler.ExecuteQuery(query, search));
    }
}
