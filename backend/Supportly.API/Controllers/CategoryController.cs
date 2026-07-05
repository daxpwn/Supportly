using Application.DTO.Search;
using Application.Queries;
using Implementation.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Supportly.API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly UseCaseHandler _handler;

        public CategoryController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        // GET: api/categories
        [HttpGet]
        [Authorize]
        public IActionResult Get([FromServices] IGetCategoriesQuery query,
                                 [FromQuery] CategorySearch search)
            => Ok(_handler.ExecuteQuery(query, search));
    }
}
