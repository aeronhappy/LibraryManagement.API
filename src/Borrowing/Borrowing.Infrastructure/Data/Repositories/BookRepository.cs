
using Borrowing.Domain.Entities;
using Borrowing.Domain.Repositories;
using Borrowing.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Borrowing.Infrastructure.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly BorrowingDbContext _context;

        public BookRepository(BorrowingDbContext context)
        {
            _context = context;
        }
        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);

        }


        public async Task<Book?> GetBookByIdAsync(BookId id)
        {
            Book? book = await _context.Books.Where((x) => x.Id == id).FirstOrDefaultAsync();
            if (book == null) { return null; }


            return book;
        }

        public async Task DeleteBookAsync(BookId id)
        {
            Book? book = await _context.Books.Where((x) => x.Id == id).FirstOrDefaultAsync();
            if (book == null) { return; }

            _context.Books.Remove(book);
        }

        public async Task UpdateBookAsync(BookId id, string title, string author, string isbn)
        {
            Book? book = await _context.Books.Where((x) => x.Id == id).FirstOrDefaultAsync();
            if (book == null) { return; }

            book.Title = title;
            book.Author = author;
            book.ISBN = isbn;

        }


    }
}
