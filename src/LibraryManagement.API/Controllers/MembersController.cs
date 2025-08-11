using FluentResults;
using LibraryManagement.API.Request;
using LibraryManagement.API.Services.Interface;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Errors;
using LibraryManagement.Application.Response;
using LibraryManagement.Infrastructure.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagement.API.Controllers
{
    [Route("api/members")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly IMemberQueries _memberQuery;
        private readonly IMemberCommands _memberCommand;

        public MembersController(IMemberQueries memberQuery, IMemberCommands memberCommand)
        {
            _memberQuery = memberQuery;
            _memberCommand = memberCommand;
        }

        
        [Authorize(Roles = "Librarian")]
        [HttpGet()]
        public async Task<ActionResult<List<MemberResponse>>> GetListOfMember([FromQuery] string searchText = "")
        {
            var listOfMembers = await _memberQuery.GetAllMemberesAsync(searchText);
            return Ok(listOfMembers);
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<MemberResponse>> GetMyAccount()
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newGuid = Guid.Parse(userId!);
            var memberResponse = await _memberQuery.GetMemberByIdAsync(newGuid);
            if (memberResponse is null)
                return NotFound();

            return Ok(memberResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberResponse>> GetMemberById(Guid id)
        {

            var memberResponse = await _memberQuery.GetMemberByIdAsync(id);
            if (memberResponse is null)
                return NotFound();

            return Ok(memberResponse);
        }

        [HttpPost()]
        public async Task<ActionResult<MemberResponse>> CreateMember(AddMemberRequest createMember)
        {
            var memberResponse = await _memberCommand.CreateMemberAsync(createMember.Name, createMember.Email, HttpContext.RequestAborted);
            return Ok(memberResponse.Value);
        }

        [Authorize(Roles = "Librarian")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<MemberResponse>> DeleteMember(Guid id)
        {
            Result result = await _memberCommand.RemoveMemberAsync(id, HttpContext.RequestAborted);
            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);
            else if (result.HasError<UnableToDeleteError>(out var unableToDeleteErrors))
                return BadRequest(unableToDeleteErrors.FirstOrDefault()?.Message);


            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMember(Guid id, AddMemberRequest createMember)
        {
            Result result = await _memberCommand.UpdateMemberAsync(id, createMember.Name, createMember.Email, HttpContext.RequestAborted);
            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);

            return Ok();
        }

        [Authorize]
        [HttpPut("profilePicture")]
        public async Task<ActionResult> UpdateProfilePic([FromForm] ProfilePictureRequest profilePictureRequest)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var newGuid = Guid.Parse(userId!);

            if (profilePictureRequest is null || profilePictureRequest.Image.Length is 0)
                return BadRequest("No file uploaded.");

            var extension = Path.GetExtension(profilePictureRequest.Image.FileName);
            var fileName = $"{Guid.NewGuid()}{extension}";


            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            var filePath = Path.Combine(uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profilePictureRequest.Image.CopyToAsync(stream);
            }

            var relativePath = $"/uploads/{fileName}";


            Result result = await _memberCommand.UpdateProfilePictureAsync(newGuid, relativePath, HttpContext.RequestAborted);

            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);

            return Ok("Profile picture updated.");
        }
    }
}
