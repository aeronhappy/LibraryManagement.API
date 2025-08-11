using FluentResults;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Errors;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Exception;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.Services;
using LibraryManagement.Domain.ValueObjects;
using System.Threading;

namespace LibraryManagement.Application.CommandHandler
{
    public class BorrowingCommands : IBorrowingCommands
    {
        private readonly IUnitOfWork _unitOfWork;

        public BorrowingCommands(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> AcceptRequestAsync(Guid requestBorrowId, CancellationToken cancellationToken)
        {
            BorrowingRequest? borrowingRequest = await _unitOfWork.BorrowingRecords.GetBorrowingRequestAsyncById(new BorrowingRequestId(requestBorrowId));
            if (borrowingRequest is null)
                return Result.Fail(new EntityNotFoundError($"Borrowing Request not found {requestBorrowId}"));

            if(borrowingRequest.IsAccepted.HasValue)
                return Result.Fail(new ConflictError($"Cannot accept borrowing request that  accept already"));

            borrowingRequest.Accept();
            await BorrowAsync(borrowingRequest.BookId.Value, borrowingRequest.BorrowerId.Value, cancellationToken);

            return Result.Ok();
        }

        public async Task<Result> BorrowAsync(Guid bookId, Guid memberId, CancellationToken cancellationToken)
        {
            try
            {
                Book? book = await _unitOfWork.Books.GetBookByIdAsync(new BookId(bookId));
                if (book is null)
                    return Result.Fail(new EntityNotFoundError($"Book not found {bookId}"));
                Member? member = await _unitOfWork.Members.GetMemberByIdAsync(new MemberId(memberId));
                if (member is null)
                    return Result.Fail(new EntityNotFoundError($"Member not found {memberId}"));

                var borrowingService = new BorrowingService();
                BorrowingRecord record = borrowingService.BorrowBook(member, book);

                await _unitOfWork.BorrowingRecords.AddAsync(record);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Ok();
            }
            catch (BookUnavailableException)
            {
                return Result.Fail(new BookUnavailableError($"Book is already borrowed."));
            } 
            catch (MemberReachLimitException)
            {
                return Result.Fail(new MemberReachLimitError($"Member has reach the limit borrowed books."));
            }

        }

        public async Task<Result> BorrowRequestAsync(Guid bookId, Guid memberId, CancellationToken cancellationToken)
        {
            Book? book = await _unitOfWork.Books.GetBookByIdAsync(new BookId(bookId));
            if (book is null)
                return Result.Fail(new EntityNotFoundError($"Book not found {bookId}"));
            Member? member = await _unitOfWork.Members.GetMemberByIdAsync(new MemberId(memberId));
            if (member is null)
                return Result.Fail(new EntityNotFoundError($"Member not found {memberId}"));

            var borrowingService = new BorrowingService();
            BorrowingRequest request = borrowingService.BorrowingBookRequest(member, book);


            await _unitOfWork.BorrowingRecords.RequestAsync(request);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Ok();
        }


        public async Task<Result> ReturnAsync(Guid bookId, Guid memberId, CancellationToken cancellationToken)
        {
            try
            {
                BorrowingRecord? record = await _unitOfWork
                    .BorrowingRecords
                    .GetByMemberAndBookIdAsync(new BookId(bookId), new MemberId(memberId));

                var borrowingService = new BorrowingService();
                borrowingService.ProcessReturn(record!);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                return Result.Ok();
            }
            catch (BookAlreadyReturnedException)
            {
                return Result.Fail(new BookAlreadyReturnedError("Can't return if book is not borrowed"));
            }
            catch (MemberCantReturnWithoutBorrowingException)
            {
                return Result.Fail(new MemberCantReturnWithoutBorrowingError("User without borrow book can't return books."));
            }
        }
    }
}
