using FluentResults;
using LibraryManagement.Application.Response;

namespace LibraryManagement.API.Services.Interface
{
    public interface IBookCommandService
    {
        Task<BookResponse?> GetBookByIdAsync(Guid bookId);
        Task<List<BookResponse>> GetAllBooksAsync(string searchText);
        Task<List<BookResponse>> GetAllAvailableBooksAsync(string searchText);
        //Task<List<BookResponse>> GetOverdueBooksAsync(string searchText);
        Task<Result<BookResponse>> AddBookAsync(string title, string author, string isbn);
        Task<Result> DeleteBookAsync(Guid bookId);
        Task<Result> UpdateBookAsync(Guid bookId, string title, string author, string isbn);
        //Task<Result> AddBorrowBookAsync(Guid memberId, Guid bookId);
        //Task<Result> ReturnBorrowBookAsync(Guid memberId, Guid bookId);

    }
}
