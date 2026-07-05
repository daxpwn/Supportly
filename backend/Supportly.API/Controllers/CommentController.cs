using Application.Commands;
using Application.DTO;
using Application.DTO.Search;
using Application.Queries;
using Implementation.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Supportly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly UseCaseHandler _handler;

        public CommentController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        // POST: api/comments
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] TicketCommentInsertDTO commentInsert, [FromServices] ITicketInsertCommentCommand cmd)
        {
            _handler.ExecuteCommand(cmd, commentInsert);
            return StatusCode(201, new { id = commentInsert.Id });
        }
    }
}
