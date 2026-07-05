using Application.DTO.Search;
using Application.Queries;
using Implementation.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Supportly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public DepartmentsController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        // GET: api/departments
        [HttpGet]
        [Authorize]
        public IActionResult Get([FromServices] IGetDepartmentsQuery query,
                                 [FromQuery] DepartmentSearch search)
            => Ok(_handler.ExecuteQuery(query, search));
    }
}
