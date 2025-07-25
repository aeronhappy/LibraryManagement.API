using FluentResults;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.API.Errors;
using LibraryManagement.API.Services;
using LibraryManagement.API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/roles")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;
        public RolesController(IRoleService roleService)
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
