using FluentResults;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.API.Errors;
using LibraryManagement.API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/members")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private IMemberService _memberService;
        public MembersController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet()]
        public async Task<ActionResult<List<MemberResponse>>> GetListOfMember([FromQuery] string searchText = "")
        {
            var listOfMembers = await _memberService.GetAllMemberesAsync(searchText);
            return Ok(listOfMembers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MemberResponse>> GetMemberById(Guid id)
        {
            var memberResponse = await _memberService.GetMemberByIdAsync(id);
            if (memberResponse is null)
                return NotFound();

            return Ok(memberResponse);
        }

        [HttpPost()]
        public async Task<ActionResult<MemberResponse>> CreateMember(CreateMemberRequest createMember)
        {
            var memberResponse = await _memberService.CreateMemberAsync(createMember.Name, createMember.Email);
            return Ok(memberResponse.Value);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<MemberResponse>> DeleteMember(Guid id)
        {
            Result result = await _memberService.RemoveMemberAsync(id);
            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);
            else if (result.HasError<UnableToDeleteError>(out var unableToDeleteErrors))
                return BadRequest(unableToDeleteErrors.FirstOrDefault()?.Message);


            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateMember(Guid id, CreateMemberRequest createMember)
        {
            Result result =   await _memberService.UpdateMemberAsync(id, createMember.Name, createMember.Email);
            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);

            return Ok();
        }
    }
}
