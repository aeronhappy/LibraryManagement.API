using FluentResults;
using Identity.Application.Commands;
using Identity.Application.Errors;
using Identity.Application.Queries;
using Identity.Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Controller
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleQueries _roleQuery;
        private readonly IRoleCommands _roleCommand;

        public RolesController(IRoleQueries roleQuery, IRoleCommands roleCommand)
        {
            _roleQuery = roleQuery;
            _roleCommand = roleCommand;
        }

        [HttpGet()]
        public async Task<ActionResult<List<RoleResponse>>> GetListOfBook()
        {
            var roles = await _roleQuery.GetAllRoleAsync();
            return Ok(roles);
        }


        [HttpGet("{roleId}")]
        public async Task<ActionResult<RoleResponse>> GetRoleById(Guid roleId)
        {
            var bookResponse = await _roleQuery.GetRoleByIdAsync(roleId);
            if (bookResponse is null)
                return NotFound();

            return Ok(bookResponse);
        }

        [HttpPost("create")]
        public async Task<ActionResult<RoleResponse>> AddRole([FromQuery] string roleName)
        {

            Result<RoleResponse> roleResponse = await _roleCommand.CreateRoleAsync(name: roleName, HttpContext.RequestAborted);

            if (roleResponse.HasError<ConflictError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);

            return Ok(roleResponse.Value);
        }


    }
}
