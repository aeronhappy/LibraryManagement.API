

using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Domain.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetBookByIdAsync(BookId id);
        Task<List<Book>> GetAllBooksAsync();
        Task AddBookAsync(Book book);
        Task DeleteBookAsync(BookId id);
        Task UpdateBookAsync(BookId id, string title, string author, string isbn);
    

    }


}
