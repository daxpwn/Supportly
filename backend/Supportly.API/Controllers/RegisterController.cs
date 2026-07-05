using Application.Commands;
using Application.DTO;
using Implementation.UseCases;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Supportly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
       
        // POST api/<RegisterController>
        [HttpPost]
        public IActionResult Post(
            [FromServices] IRegisterUserCommand cmd,
            [FromServices] UseCaseHandler handler,
            [FromBody] RegisterUserDTO dto)
        {
            handler.ExecuteCommand(cmd, dto);
            return StatusCode(201);
        }
    }
}
