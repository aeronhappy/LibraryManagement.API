using AutoMapper;
using Borrowing.Application.Commands;
using Borrowing.Application.Errors;
using Borrowing.Application.Response;
using Borrowing.Domain.Entities;
using Borrowing.Domain.Repositories;
using Borrowing.Domain.ValueObjects;
using FluentResults;

namespace Borrowing.Application.CommandHandler
{
    public class BookCommands : IBookCommands
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookCommands(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Result<BookResponse>> AddBookAsync(string title, string author, string isbn, CancellationToken cancellationToken)
        {
            var newBook = new Book()
            {
                Id = new BookId(Guid.NewGuid()),
                Title = title,
                Author = author,
                ISBN = isbn,
                IsBorrowed = false,
            };
            var newBookResponse = _mapper.Map<BookResponse>(newBook);
            await _unitOfWork.Books.AddBookAsync(newBook);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok(newBookResponse);
        }


        public async Task<Result> DeleteBookAsync(Guid bookId, CancellationToken cancellationToken)
        {
            Book? book = await _unitOfWork.Books.GetBookByIdAsync(new BookId(bookId));
            if (book is null)
                return Result.Fail(new EntityNotFoundError($"No Book ={bookId} found"));

            if (book.IsBorrowed)
                return Result.Fail(new UnableToDeleteError($"Book ={bookId} is already borrowed"));

            await _unitOfWork.Books.DeleteBookAsync(new BookId(bookId));
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }

        public async Task<Result> UpdateBookAsync(Guid bookId, string title, string author, string isbn, CancellationToken cancellationToken)
        {
            Book? book = await _unitOfWork.Books.GetBookByIdAsync(new BookId(bookId));
            if (book is null)
                return Result.Fail(new EntityNotFoundError($"No Book ={bookId} found"));

            await _unitOfWork.Books.UpdateBookAsync(new BookId(bookId), title, author, isbn);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }






    }
}
