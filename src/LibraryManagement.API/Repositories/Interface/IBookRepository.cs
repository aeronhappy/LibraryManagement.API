using LibraryManagement.API.Datas.Models;

namespace LibraryManagement.API.Repositories.Interface
{
    public interface IBookRepository
    {
        Task<Book?> GetBookByIdAsync(Guid id);
        Task<List<Book>> GetAllBooksAsync();
        Task AddBookAsync(Book book);
        Task DeleteBookAsync(Guid id);
        Task UpdateBookAsync( Guid id, string title, string author, string isbn);
        Task AddBorrowBookAsync(Guid bookId, Guid memberId);
        Task RemoveBorrowedBookAsync(Guid bookId);

    }


}
