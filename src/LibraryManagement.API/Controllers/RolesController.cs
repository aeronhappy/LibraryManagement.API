using FluentResults;
using LibraryManagement.API.Services.Interface;
using LibraryManagement.Application.Errors;
using LibraryManagement.Application.Response;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleCommandService _roleService;
        public RolesController(IRoleCommandService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet()]
        public async Task<ActionResult<List<RoleResponse>>> GetListOfBook()
        {
            var roles = await _roleService.GetAllRoleAsync();
            return Ok(roles);
        }


        [HttpGet("{roleId}")]
        public async Task<ActionResult<RoleResponse>> GetRoleById(Guid roleId)
        {
            var bookResponse = await _roleService.GetRoleByIdAsync(roleId);
            if (bookResponse is null)
                return NotFound();

            return Ok(bookResponse);
        }

        [HttpPost("create")]
        public async Task<ActionResult<RoleResponse>> AddRole([FromQuery] String roleName)
        {
           
            Result<RoleResponse> roleResponse = await _roleService.CreateRoleAsync(name: roleName);

            if (roleResponse.HasError<ConflictError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);

            return Ok(roleResponse.Value);
        }


    }
}
