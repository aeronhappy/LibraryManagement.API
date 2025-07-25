using AutoMapper.Execution;
using LibraryManagement.API.Datas;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Repositories.Implementation
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
            return await _context.Books.Include(b => b.Borrower).ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(Guid id)
        {
            Book? book = await _context.Books.Where((x) => x.Id == id).Include(b => b.Borrower).FirstOrDefaultAsync();
            if (book == null) { return null; }


            return book;
        }

        public async Task DeleteBookAsync(Guid id)
        {
            Book? book = await _context.Books.Where((x) => x.Id == id).FirstOrDefaultAsync();
            if (book == null) { return; }

            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBookAsync(Guid id, string title, string author, string isbn)
        {
            Book? book = await _context.Books.Where((x) => x.Id == id).FirstOrDefaultAsync();
            if (book == null) { return; }

            book.Title = title;
            book.Author = author;
            book.ISBN = isbn;

            await _context.SaveChangesAsync();
        }



        public async Task AddBorrowBookAsync(Guid bookId, Guid memberId)
        {

            Book? book = await _context.Books.Where((x) => x.Id == bookId).FirstOrDefaultAsync();
            if (book == null) { return; }

            book.BorrowerId = memberId;
            book.IsBorrowed = book.BorrowerId.HasValue;
            book.DateBorrowed = DateTime.UtcNow;
            book.DueDate = DateTime.UtcNow.AddDays(3);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveBorrowedBookAsync(Guid bookId)
        {

            Book? book = await _context.Books.Where((x) => x.Id == bookId).FirstOrDefaultAsync();
            if (book == null) { return; }

            book.BorrowerId = null;
            book.IsBorrowed = book.BorrowerId.HasValue;
            book.DateBorrowed = null;
            book.DueDate = null;

            await _context.SaveChangesAsync();
        }

        public async Task SaveChangeAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
