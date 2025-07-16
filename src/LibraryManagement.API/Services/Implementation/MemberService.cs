using AutoMapper;
using FluentResults;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.API.Errors;
using LibraryManagement.API.Repositories.Interface;
using LibraryManagement.API.Services.Interface;
using System.Net;

namespace LibraryManagement.API.Services.Implementation
{
    public class MemberService : IMemberService
    {
        private IMemberRepository _memberRepository;
        private IMapper _mapper;

        public MemberService(IMemberRepository memberRepository, IMapper mapper)
        {
            _memberRepository = memberRepository;
            _mapper = mapper;
        }


        public async Task<List<MemberResponse>> GetAllMemberesAsync(string searchText)
        {
            var listOfMember = await _memberRepository.GetAllMemberesAsync();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                listOfMember = listOfMember
                    .Where(m =>
                        (!string.IsNullOrEmpty(m.Name) && m.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase)) ||
                        (!string.IsNullOrEmpty(m.Email) && m.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    )
                    .ToList();
            }

            List<MemberResponse> responseMembers = listOfMember
                .ConvertAll(x => _mapper.Map<MemberResponse>(x));

            return responseMembers;
        }

        public async Task<MemberResponse?> GetMemberByIdAsync(Guid id)
        {

            Member? member = await _memberRepository.GetMemberByIdAsync(id);
            if (member == null) { return null; }

            var memberResponse = _mapper.Map<MemberResponse>(member);
            return memberResponse;

        }

        public async Task<Result<MemberResponse>> CreateMemberAsync(string name, string email)
        {
            var newMember = new Member()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Email = email,
                MaxBooksAllowed = 5,
                BorrowedBooksCount = 0,
                BorrowedBooks = [],
            };

            var newMemberResponse = _mapper.Map<MemberResponse>(newMember);
            await _memberRepository.CreateMemberAsync(newMember);

            return Result.Ok(newMemberResponse);
        }



        public async Task<Result> RemoveMemberAsync(Guid id)
        {

            Member? member = await _memberRepository.GetMemberByIdAsync(id);

            if (member is null)
                return Result.Fail(new EntityNotFoundError($"No Member ={id} found"));

            if (member.BorrowedBooksCount != 0)
                return Result.Fail(new UnableToDeleteError($"Member ={id} has borrowed books"));


            await _memberRepository.RemoveMemberAsync(id);
            return Result.Ok();
        }

        public async Task<Result> UpdateMemberAsync(Guid id, string name, string email)
        {

            Member? member = await _memberRepository.GetMemberByIdAsync(id);
            if (member is null)
                return Result.Fail(new EntityNotFoundError($"No Member ={id} found"));

            await _memberRepository.UpdateMemberAsync(id, name, email);
            return Result.Ok();
        }


    }
}
