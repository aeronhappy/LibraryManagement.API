

using Borrowing.Domain.Entities;
using Borrowing.Domain.ValueObjects;

namespace Borrowing.Domain.Repositories
{
    public interface IBookRepository
    {
        Task<Book?> GetBookByIdAsync(BookId id);
        Task AddBookAsync(Book book);
        Task DeleteBookAsync(BookId id);
        Task UpdateBookAsync(BookId id, string title, string author, string isbn);


    }


}
