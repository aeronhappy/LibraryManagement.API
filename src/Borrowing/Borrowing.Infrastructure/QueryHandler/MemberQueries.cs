using AutoMapper;
using AutoMapper.QueryableExtensions;
using Borrowing.Application.Queries;
using Borrowing.Application.Response;
using Borrowing.Domain.Entities;
using Borrowing.Domain.ValueObjects;
using Borrowing.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Borrowing.Infrastructure.QueryHandler
{
    public class MemberQueries : IMemberQueries
    {
        private readonly BorrowingDbContext _context;
        private IMapper _mapper;

        public MemberQueries(BorrowingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        public async Task<List<MemberResponse>> GetAllMemberesAsync(string searchText)
        {

            IQueryable<Member> query = _context.Members.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var loweredSearchText = searchText.ToLower();
                query = query.Where(m => m.Name.Contains(loweredSearchText)
                                      || m.Email.Contains(loweredSearchText));
            }

            return await query.ProjectTo<MemberResponse>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<MemberResponse?> GetMemberByIdAsync(Guid id)
        {

            var member = await _context.Members
                 .Where(m => m.Id == new MemberId(id))
                 .ProjectTo<MemberResponse>(_mapper.ConfigurationProvider)
                 .FirstOrDefaultAsync();

            return member;
        }


    }
}
