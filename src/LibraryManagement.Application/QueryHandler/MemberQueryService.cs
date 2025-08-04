using AutoMapper;
using LibraryManagement.Application.Queries;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Application.CommandHandler
{
    public class MemberQueryService : IMemberQueryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public MemberQueryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public async Task<List<MemberResponse>> GetAllMemberesAsync(string searchText)
        {
            var listOfMember = await _unitOfWork.Members.GetAllMemberesAsync();

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
            
            Member? member = await _unitOfWork.Members.GetMemberByIdAsync(new MemberId(id));
            if (member == null) { return null; }

            var memberResponse = _mapper.Map<MemberResponse>(member);
            return memberResponse;

        }

     
    }
}
