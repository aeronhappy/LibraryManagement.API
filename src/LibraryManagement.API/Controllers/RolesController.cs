using FluentResults;
using LibraryManagement.API.Services.Interface;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Errors;
using LibraryManagement.Application.Response;
using LibraryManagement.Infrastructure.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleQueries _roleQuery;
        private readonly IRoleCommands _roleCommand;

        public RolesController(IRoleQueries roleQuery,IRoleCommands roleCommand)
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
        public async Task<ActionResult<RoleResponse>> AddRole([FromQuery] String roleName)
        {
           
            Result<RoleResponse> roleResponse = await _roleCommand.CreateRoleAsync(name: roleName, HttpContext.RequestAborted);

            if (roleResponse.HasError<ConflictError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);

            return Ok(roleResponse.Value);
        }


    }
}
