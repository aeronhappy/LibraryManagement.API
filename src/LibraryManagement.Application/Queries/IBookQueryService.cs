using LibraryManagement.Application.Response;

namespace LibraryManagement.Application.Queries
{
    public interface IBookQueryService
    {
        Task<BookResponse?> GetBookByIdAsync(Guid bookId);
        Task<List<BookResponse>> GetAllBooksAsync(string searchText);
        Task<List<BookResponse>> GetAllAvailableBooksAsync(string searchText);
        Task<List<BookResponse>> GetOverdueBooksAsync(string searchText);

    }
}
