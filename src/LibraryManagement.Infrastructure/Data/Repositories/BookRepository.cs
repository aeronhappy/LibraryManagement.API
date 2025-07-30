
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Data.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddBookAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _context.Books.ToListAsync();
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
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(BookId id, string title, string author, string isbn)
        {
            Book? book = await _context.Books.Where((x) => x.Id == id).FirstOrDefaultAsync();
            if (book == null) { return; }

            book.Title = title;
            book.Author = author;
            book.ISBN = isbn;

            await _context.SaveChangesAsync();
        }



        //public async Task AddBorrowBookAsync(Guid bookId, Guid memberId)
        //{

        //    Book? book = await _context.Books.Where((x) => x.Id == bookId).FirstOrDefaultAsync();
        //    if (book == null) { return; }

        //    book.BorrowerId = memberId;
        //    book.IsBorrowed = book.BorrowerId.HasValue;
        //    book.DateBorrowed = DateTime.UtcNow;
        //    book.DueDate = DateTime.UtcNow.AddDays(3);

        //    await _context.SaveChangesAsync();
        //}

        //public async Task RemoveBorrowedBookAsync(Guid bookId)
        //{

        //    Book? book = await _context.Books.Where((x) => x.Id == bookId).FirstOrDefaultAsync();
        //    if (book == null) { return; }

        //    book.BorrowerId = null;
        //    book.IsBorrowed = book.BorrowerId.HasValue;
        //    book.DateBorrowed = null;
        //    book.DueDate = null;

        //    await _context.SaveChangesAsync();
        //}

        //public async Task SaveChangeAsync()
        //{
        //    await _context.SaveChangesAsync();
        //}
    }
}
