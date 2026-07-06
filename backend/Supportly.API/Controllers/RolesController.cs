using Application.Commands;
using Application.DTO;
using Application.Queries;
using Implementation.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Supportly.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly UseCaseHandler _handler;

        public RolesController(UseCaseHandler handler)
        {
            _handler = handler;
        }

        // GET: api/roles — sve role sa svojim use case-ovima
        [HttpGet]
        [Authorize]
        public IActionResult Get([FromServices] IGetRolesQuery query)
            => Ok(_handler.ExecuteQuery(query, (object)null));

        // GET: api/roles/usecases — katalog svih dostupnih use case id-jeva
        [HttpGet("usecases")]
        [Authorize]
        public IActionResult Catalog([FromServices] IGetUseCaseCatalogQuery query)
            => Ok(_handler.ExecuteQuery(query, (object)null));

        // POST: api/roles/5/usecases  body: { "useCaseId": "get-tickets" }
        [HttpPost("{roleId}/usecases")]
        [Authorize]
        public IActionResult AddUseCase([FromRoute] byte roleId,
                                        [FromBody] RoleUseCaseDTO dto,
                                        [FromServices] IAddRoleUseCaseCommand cmd)
        {
            dto.RoleId = roleId;
            _handler.ExecuteCommand(cmd, dto);
            return NoContent();
        }

        // DELETE: api/roles/5/usecases/get-tickets
        [HttpDelete("{roleId}/usecases/{useCaseId}")]
        [Authorize]
        public IActionResult RemoveUseCase([FromRoute] byte roleId,
                                           [FromRoute] string useCaseId,
                                           [FromServices] IRemoveRoleUseCaseCommand cmd)
        {
            _handler.ExecuteCommand(cmd, new RoleUseCaseDTO { RoleId = roleId, UseCaseId = useCaseId });
            return NoContent();
        }
    }
}
