using FluentResults;
using LibraryManagement.Application.Response;

namespace LibraryManagement.API.Services.Interface
{
    public interface IBookCommands
    {
      
        Task<Result<BookResponse>> AddBookAsync(string title, string author, string isbn, CancellationToken cancellationToken);
        Task<Result> DeleteBookAsync(Guid bookId, CancellationToken cancellationToken);
        Task<Result> UpdateBookAsync(Guid bookId, string title, string author, string isbn, CancellationToken cancellationToken);
     

    }
}
