using Borrowing.Application.Commands;
using Borrowing.Application.Errors;
using Borrowing.Application.Queries;
using Borrowing.Application.Response;
using Borrowing.Controller.Request;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Borrowing.Controller
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

        /// <summary>
        /// Retrieves all members, optionally filtered by search text. (Librarian/Admin)
        /// </summary>
        /// <param name="searchText">Search filter to match member name or email.</param>
        /// <returns>A list of members.</returns>
        [Authorize(Roles = "Librarian,Admin")]
        [HttpGet()]
        public async Task<ActionResult<List<MemberResponse>>> GetListOfMember([FromQuery] string searchText = "")
        {
            var listOfMembers = await _memberQuery.GetAllMemberesAsync(searchText);
            return Ok(listOfMembers);
        }

     

        /// <summary>
        /// Retrieves a member by their unique identifier. (Librarian/Admin)
        /// </summary>
        /// <param name="id">The ID of the member to retrieve.</param>
        /// <returns>The requested member, or 404 if not found.</returns>
        [Authorize(Roles = "Librarian,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<MemberResponse>> GetMemberById(Guid id)
        {
            var memberResponse = await _memberQuery.GetMemberByIdAsync(id);
            if (memberResponse is null)
                return NotFound();

            return Ok(memberResponse);
        }


        /// <summary>
        /// Deletes an existing member by their unique identifier. (Librarian/Admin)
        /// </summary>
        /// <param name="id">The ID of the member to delete.</param>
        /// <returns>No content if successful, or error details if failed.</returns>
        [Authorize(Roles = "Librarian,Admin")]
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

        /// <summary>
        /// Retrieves the currently authenticated member's account information. (Authenticated User)
        /// </summary>
        /// <returns>The current member's account details.</returns>
        [Authorize]
        [HttpGet("me")]
        public async Task<ActionResult<MemberResponse>> GetMyAccount()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var newGuid = Guid.Parse(userId!);
            var memberResponse = await _memberQuery.GetMemberByIdAsync(newGuid);

            if (memberResponse is null)
                return NotFound();

            return Ok(memberResponse);
        }



        /// <summary>
        /// Updates name or email of the currently authenticated member. (Authenticated User)
        /// </summary>
        [Authorize]
        [HttpPut("me")]
        public async Task<ActionResult> UpdateMember([FromQuery] string name , [FromQuery] string email)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var newGuid = Guid.Parse(userId!);

            Result result = await _memberCommand.UpdateMemberAsync(newGuid, name, email, HttpContext.RequestAborted);
            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);

            return Ok();
        }

        /// <summary>
        /// Updates the profile picture of the currently authenticated member. (Authenticated User)
        /// </summary>
        /// <param name="profilePictureRequest">The profile picture file to upload.</param>
        /// <returns>Confirmation message if successful, or error details if failed.</returns>
        [Authorize]
        [HttpPut("profilePicture")]
        public async Task<ActionResult> UpdateProfilePic([FromForm] ProfilePictureRequest profilePictureRequest)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
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
