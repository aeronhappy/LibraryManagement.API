using LibraryManagement.Application.Response;

namespace LibraryManagement.Infrastructure.Queries
{
    public interface IBookQueries
    {
        Task<BookResponse?> GetBookByIdAsync(Guid bookId);
        Task<List<BookResponse>> GetAllBooksAsync(string searchText);
        Task<List<BookResponse>> GetAllAvailableBooksAsync(string searchText);
        Task<List<BookResponse>> GetOverdueBooksAsync(string searchText);

    }
}
