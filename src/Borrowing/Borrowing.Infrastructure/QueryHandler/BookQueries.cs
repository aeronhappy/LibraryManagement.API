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
    public class BookQueries : IBookQueries
    {
        private readonly BorrowingDbContext _context;
        private IMapper _mapper;

        public BookQueries(BorrowingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        public async Task<List<BookResponse>> GetAllBooksAsync(string searchText)
        {
            IQueryable<Book> query = _context.Books.AsQueryable();
            if (!string.IsNullOrWhiteSpace(searchText))
            {

                query = query.Where(book => book.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                                || book.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }
            return await query.ProjectTo<BookResponse>(_mapper.ConfigurationProvider).ToListAsync();

        }

        public async Task<List<BookResponse>> GetAllAvailableBooksAsync(string searchText)
        {
            IQueryable<Book> query = _context.Books.Where(b => !b.IsBorrowed);
            if (!string.IsNullOrWhiteSpace(searchText))
            {

                query = query.Where(book => book.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                                || book.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }
            return await query.ProjectTo<BookResponse>(_mapper.ConfigurationProvider).ToListAsync();

        }

        public async Task<List<BookResponse>> GetOverdueBooksAsync(string searchText)
        {

            IQueryable<Book> query = _context.Books.Where(b => b.BorrowingHistory.Any(br => br.DateReturned == null && br.DateOverdue < DateTime.UtcNow));
            if (!string.IsNullOrWhiteSpace(searchText))
            {

                var loweredSearchText = searchText.ToLower();
                query = query.Where(book => book.Title.Contains(loweredSearchText)
                                         || book.Author.Contains(loweredSearchText));
            }
            return await query.ProjectTo<BookResponse>(_mapper.ConfigurationProvider).ToListAsync();

        }


        public async Task<BookResponse?> GetBookByIdAsync(Guid bookId)
        {
            var book = await _context.Books
                .Where(b => b.Id == new BookId(bookId))
                .ProjectTo<BookResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return book;
        }





    }
}
