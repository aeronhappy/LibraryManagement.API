using FluentResults;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.API.Errors;
using LibraryManagement.API.Services;
using LibraryManagement.API.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagement.API.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost("register")]

        public async Task<ActionResult> Register([FromBody] RegisterUserRequest request)
        {
            var response =
                await _authenticationService.RegisterAsync(request.Name, request.Email, request.Password, request.RolesId);
            return Ok(response);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var response = await _authenticationService.SignInAsync(request.Email, request.Password);
            if (!response.IsSuccess)
            {
                return BadRequest(response);
            }
            return Ok(response);

        }




        [HttpGet()]
        public async Task<ActionResult> GetAllUser()
        {
            List<UserResponse> userResponses = await _authenticationService.GetAllUsersAsync();
            return Ok(userResponses);
        }

        [HttpGet("me")]
        public async Task<ActionResult> GetMyAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newGuid = Guid.Parse(userId!);
            UserResponse? userResponse = await _authenticationService.GetUserByIdAsync(newGuid);
            return Ok(userResponse);
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserById(Guid userId)
        {
            UserResponse? userResponse = await _authenticationService.GetUserByIdAsync(userId);
            return Ok(userResponse);
        }

    }
}
