using AutoMapper;
using FluentResults;
using LibraryManagement.API.Services.Interface;
using LibraryManagement.Application.Errors;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Application.CommandHandler
{
    public class BookCommandService : IBookCommandService
    {
        private IBookRepository _bookRepository;
        private IMemberRepository _memberRepository;
        private IMapper _mapper;

        public BookCommandService(IBookRepository bookRepository, IMemberRepository memberRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
            _mapper = mapper;

        }
        public async Task<Result<BookResponse>> AddBookAsync(string title, string author, string isbn)
        {
            var newBook = new Book()
            {
                Id = new BookId(Guid.NewGuid()) ,
                Title = title,
                Author = author,
                ISBN = isbn,
                IsBorrowed = false,
            };


            var newBookResponse = _mapper.Map<BookResponse>(newBook);
            await _bookRepository.AddBookAsync(newBook);

            return Result.Ok(newBookResponse);
        }


        public async Task<List<BookResponse>> GetAllBooksAsync(string searchText)
        {
            List<Book> listOfBooks = await _bookRepository.GetAllBooksAsync();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                listOfBooks = listOfBooks
                    .Where(book => book.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                                || book.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return listOfBooks.ConvertAll(book => _mapper.Map<BookResponse>(book));

        }

        public async Task<List<BookResponse>> GetAllAvailableBooksAsync(string searchText)
        {
            List<Book> listOfBooks = await _bookRepository.GetAllBooksAsync();

            listOfBooks = listOfBooks.Where(book => !book.IsBorrowed).ToList();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                listOfBooks = listOfBooks
                    .Where(book => book.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                                || book.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return listOfBooks.Select(book => _mapper.Map<BookResponse>(book)).ToList();
        }

        //public async Task<List<BookResponse>> GetOverdueBooksAsync(string searchText)
        //{
        //    List<Book> listOfBooks = await _bookRepository.GetAllBooksAsync();

        //    DateTime now = DateTime.Now;
        //    listOfBooks = listOfBooks.Where(book => now >= book.DueDate).ToList();

        //    if (!string.IsNullOrWhiteSpace(searchText))
        //    {
        //        listOfBooks = listOfBooks
        //            .Where(book => book.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)
        //                        || book.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase))
        //            .ToList();
        //    }

        //    return listOfBooks.Select(book => _mapper.Map<BookResponse>(book)).ToList();
        //}


        public async Task<BookResponse?> GetBookByIdAsync(Guid bookId)
        {
            Book? book = await _bookRepository.GetBookByIdAsync(new BookId(bookId));
            if (book is null)
                return null;

            var newBookResponse = _mapper.Map<BookResponse>(book);

            return newBookResponse;
        }

        public async Task<Result> DeleteBookAsync(Guid bookId)
        {
            Book? book = await _bookRepository.GetBookByIdAsync(new BookId(bookId));
            if (book is null)
                return Result.Fail(new EntityNotFoundError($"No Book ={bookId} found"));

            if (book.IsBorrowed)
                return Result.Fail(new UnableToDeleteError($"Book ={bookId} is already borrowed"));

            await _bookRepository.DeleteBookAsync(new BookId(bookId));
            return Result.Ok();
        }

        public async Task<Result> UpdateBookAsync(Guid bookId, string title, string author, string isbn)
        {
            Book? book = await _bookRepository.GetBookByIdAsync(new BookId(bookId));
            if (book is null)
                return Result.Fail(new EntityNotFoundError($"No Book ={bookId} found"));

            await _bookRepository.UpdateBookAsync(new BookId(bookId), title, author, isbn);
            return Result.Ok();
        }




        //public async Task<Result> AddBorrowBookAsync(Guid memberId, Guid bookId)
        //{
        //    Member? member = await _memberRepository.GetMemberByIdAsync(new MemberId(memberId));
        //    Book? book = await _bookRepository.GetBookByIdAsync(new BookId(bookId));

        //    if (book is null)
        //        return Result.Fail(new EntityNotFoundError($"No Book ={bookId} found"));
        //    if (member is null)
        //        return Result.Fail(new EntityNotFoundError($"No Member ={memberId} found"));
        //    if (book.IsBorrowed)
        //        return Result.Fail(new BookUnavailableError($"Book = {bookId} is already borrowed"));
        //    if (member.MaxBooksAllowed == member.BorrowedBooksCount)
        //        return Result.Fail(new MemberReachLimitError($"Member = {memberId} has reached the borrowing limit of {member.MaxBooksAllowed} books"));


        //    //await _memberRepository.AddBorrowedBookAsync(memberId, book);
        //    //await _bookRepository.AddBorrowBookAsync(bookId, memberId);

        //    return Result.Ok();
        //}


        //public async Task<Result> ReturnBorrowBookAsync(Guid memberId, Guid bookId)
        //{
        //    Member? member = await _memberRepository.GetMemberByIdAsync(memberId);
        //    Book? book = await _bookRepository.GetBookByIdAsync(bookId);

        //    if (book is null)
        //        return Result.Fail(new EntityNotFoundError($"No Book ={bookId} found"));
        //    if (member is null)
        //        return Result.Fail(new EntityNotFoundError($"No Member ={memberId} found"));
        //    if (book.BorrowerId != memberId)
        //        return Result.Fail(new EntityNotFoundError($"Book = {bookId} was not borrowed by Member = {memberId}"));


        //    await _memberRepository.RemoveBorrowedBookAsync(memberId, book);
        //    await _bookRepository.RemoveBorrowedBookAsync(bookId);

        //    return Result.Ok();

        //}


    }
}
