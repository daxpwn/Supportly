using Application.Commands;
using Application.DTO;
using Application.DTO.Search;
using Application.Queries;
using Implementation.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace Supportly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public TicketsController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get([FromServices] IGetTicketsQuery query,
                                 [FromQuery] TicketSearch search)
            => Ok(_handler.ExecuteQuery(query, search));

        // GET: api/tickets/my  — tiketi ulogovanog klijenta (samo njegovi)
        [HttpGet("my")]
        [Authorize]
        public IActionResult GetMy([FromServices] IGetMyTicketsQuery query,
                                   [FromQuery] TicketSearch search)
            => Ok(_handler.ExecuteQuery(query, search));

        [HttpGet("{id:long}")]
        [Authorize]
        public IActionResult Get([FromServices] IGetTicketQuery query,
                         [FromRoute] long id)
        => Ok(_handler.ExecuteQuery(query, id));


        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] TicketInsertDTO ticketInsert, [FromServices] ITicketInsertCommand cmd)
        {
            _handler.ExecuteCommand(cmd, ticketInsert);
            return StatusCode(201, new { id = ticketInsert.Id, ticketNumber = ticketInsert.TicketNumber });
        }

        // PATCH: api/tickets/5/status  — promena statusa (osoblje; TicketId iz rute)
        [HttpPatch("{id}/status")]
        [Authorize]
        public IActionResult ChangeStatus([FromRoute] long id,
                                          [FromBody] ChangeTicketStatusDTO dto,
                                          [FromServices] IChangeTicketStatusCommand cmd)
        {
            dto.TicketId = id;
            _handler.ExecuteCommand(cmd, dto);
            return NoContent();
        }

        // POST: api/tickets/5/attachments  — upload priloga (multipart/form-data, polje "file")
        // Opciono form polje "commentId" -> prilog se veže i za konkretan komentar tog tiketa.
        [HttpPost("{id}/attachments")]
        [Authorize]
        public IActionResult UploadAttachment([FromRoute] long id,
                                              IFormFile file,
                                              [FromForm] long? commentId,
                                              [FromServices] IUploadAttachmentCommand cmd)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File is required.");

            using var ms = new MemoryStream();
            file.CopyTo(ms);

            var dto = new AttachmentUploadDTO
            {
                TicketId = id,
                CommentId = commentId,
                FileName = file.FileName,
                ContentType = file.ContentType,
                Content = ms.ToArray()
            };

            _handler.ExecuteCommand(cmd, dto);
            return StatusCode(201);
        }
    }
}
