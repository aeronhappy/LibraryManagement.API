using LibraryManagement.API.Datas;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace LibraryManagement.API.Repositories.Implementation
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContext _context;

        public MemberRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<List<Member>> GetAllMemberesAsync()
        {
            return await _context.Members.Include(b => b.BorrowedBooks).ToListAsync();
        }


        public async Task<Member?> GetMemberByIdAsync(Guid id)
        {
            Member? member = await _context.Members.Where((x) => x.Id == id).FirstOrDefaultAsync();
            if (member == null) { return null; }


            return member;
        }


        public async Task CreateMemberAsync(Member memberModel)
        {
            await _context.Members.AddAsync(memberModel);
            await _context.SaveChangesAsync();

        }

        public async Task RemoveMemberAsync(Guid id)
        {
            Member? member = await _context.Members.Where((x) => x.Id == id).FirstOrDefaultAsync();
            if (member == null) { return; }


            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMemberAsync(Guid id, string name, string email)
        {
            Member? member = await _context.Members.Where((x) => x.Id == id).FirstOrDefaultAsync();
            if (member == null) { return; }


            member.Name = name;
            member.Email = email;

            await _context.SaveChangesAsync();
        }
        public async Task AddBorrowedBookAsync(Guid id, Book borrowedBook)
        {
            Member? member = await _context.Members.Where((x) => x.Id == id).FirstOrDefaultAsync();
            if (member == null) { return; }

            member.BorrowedBooks.Add(borrowedBook);
            member.BorrowedBooksCount++;

            await _context.SaveChangesAsync();
        }

        public async Task RemoveBorrowedBookAsync(Guid id, Book borrowedBook)
        {
            Member? member = await _context.Members.Where((x) => x.Id == id).FirstOrDefaultAsync();
            if (member == null) { return; }

            if (member.BorrowedBooks.Contains(borrowedBook))
            {
                member.BorrowedBooks.Remove(borrowedBook);
            }
            member.BorrowedBooksCount--;

            await _context.SaveChangesAsync();
        }
    }
}
