using Application.Commands;
using Application.DTO;
using Application.DTO.Search;
using Application.Queries;
using Implementation.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Supportly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UseCaseHandler _handler;

        public UsersController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        // GET: api/users  — svi korisnici (admin)
        [HttpGet]
        [Authorize]
        public IActionResult Get([FromServices] IGetUsersQuery query,
                                 [FromQuery] UserSearch search)
            => Ok(_handler.ExecuteQuery(query, search));

        // GET: api/users/5  — jedan korisnik
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult GetOne([FromRoute] int id, [FromServices] IGetUserQuery query)
        {
            var user = _handler.ExecuteQuery(query, id);
            return user is null ? NotFound() : Ok(user);
        }

        // PUT: api/users/5  — izmena korisnika
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Update([FromRoute] int id,
                                    [FromBody] UserUpdateDTO dto,
                                    [FromServices] IUpdateUserCommand cmd)
        {
            dto.Id = id;
            _handler.ExecuteCommand(cmd, dto);
            return NoContent();
        }

        // DELETE: api/users/5  — brisanje korisnika
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete([FromRoute] int id, [FromServices] IDeleteUserCommand cmd)
        {
            _handler.ExecuteCommand(cmd, id);
            return NoContent();
        }
    }
}
