using LibraryManagement.API.Request;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Response;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagement.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationCommands _authenticationService;

        public AuthenticationController(IAuthenticationCommands authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]

        public async Task<ActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var response =
                await _authenticationService.RegisterAsync(request.Name, request.Email, request.Password, request.RolesId, HttpContext.RequestAborted);
            return Ok(response);
        }
       
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authenticationService.SignInAsync(request.Email, request.Password, HttpContext.RequestAborted);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }


    }
}
