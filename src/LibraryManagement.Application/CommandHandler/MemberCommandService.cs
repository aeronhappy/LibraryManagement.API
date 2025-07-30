using AutoMapper;
using FluentResults;
using LibraryManagement.API.Services.Interface;
using LibraryManagement.Application.Errors;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Application.CommandHandler
{
    public class MemberCommandService : IMemberCommandService
    {
        private IMemberRepository _memberRepository;
        private IMapper _mapper;

        public MemberCommandService(IMemberRepository memberRepository, IMapper mapper)
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
                        !string.IsNullOrEmpty(m.Name) && m.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                        !string.IsNullOrEmpty(m.Email) && m.Email.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                    )
                    .ToList();
            }

            List<MemberResponse> responseMembers = listOfMember
                .ConvertAll(x => _mapper.Map<MemberResponse>(x));

            return responseMembers;
        }

        public async Task<MemberResponse?> GetMemberByIdAsync(Guid id)
        {

            Member? member = await _memberRepository.GetMemberByIdAsync(new MemberId(id));
            if (member == null) { return null; }

            var memberResponse = _mapper.Map<MemberResponse>(member);
            return memberResponse;

        }

        public async Task<Result<MemberResponse>> CreateMemberAsync(string name, string email)
        {
            var newMember = new Member()
            {
                Id = new MemberId(Guid.NewGuid()) ,
                Name = name,
                Email = email,
                MaxBooksAllowed = 5,
                BorrowedBooksCount = 0,
              
            };

            var newMemberResponse = _mapper.Map<MemberResponse>(newMember);
            await _memberRepository.CreateMemberAsync(newMember);

            return Result.Ok(newMemberResponse);
        }



        public async Task<Result> RemoveMemberAsync(Guid id)
        {

            Member? member = await _memberRepository.GetMemberByIdAsync(new MemberId(id));

            if (member is null)
                return Result.Fail(new EntityNotFoundError($"No Member ={id} found"));

            if (member.BorrowedBooksCount != 0)
                return Result.Fail(new UnableToDeleteError($"Member ={id} has borrowed books"));


            await _memberRepository.RemoveMemberAsync(new MemberId(id));
            return Result.Ok();
        }

        public async Task<Result> UpdateMemberAsync(Guid id, string name, string email)
        {

            Member? member = await _memberRepository.GetMemberByIdAsync(new MemberId(id));
            if (member is null)
                return Result.Fail(new EntityNotFoundError($"No Member ={id} found"));

            await _memberRepository.UpdateMemberAsync(new MemberId(id), name, email);
            return Result.Ok();
        }


    }
}
