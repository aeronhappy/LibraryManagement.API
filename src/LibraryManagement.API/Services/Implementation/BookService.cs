using AutoMapper;
using FluentResults;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.API.Errors;
using LibraryManagement.API.Profiles;
using LibraryManagement.API.Repositories;
using LibraryManagement.API.Repositories.Interface;
using LibraryManagement.API.Services.Interface;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Net;

namespace LibraryManagement.API.Services.Implementation
{
    public class BookService : IBookService
    {
        private IBookRepository _bookRepository;
        private IMemberRepository _memberRepository;
        private IMapper _mapper;

        public BookService(IBookRepository bookRepository, IMemberRepository memberRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _memberRepository = memberRepository;
            _mapper = mapper;

        }
        public async Task<Result<BookResponse>> AddBookAsync(string title, string author, string isbn)
        {
            var newBook = new Book()
            {
                Id = Guid.NewGuid(),
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

        public async Task<List<BookResponse>> GetOverdueBooksAsync(string searchText)
        {
            List<Book> listOfBooks = await _bookRepository.GetAllBooksAsync();

            DateTime now = DateTime.Now;
            listOfBooks = listOfBooks.Where(book => now >= book.DueDate).ToList();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                listOfBooks = listOfBooks
                    .Where(book => book.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                                || book.Author.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return listOfBooks.Select(book => _mapper.Map<BookResponse>(book)).ToList();
        }


        public async Task<BookResponse?> GetBookByIdAsync(Guid bookId)
        {
            Book? book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book is null)
                return null;

            var newBookResponse = _mapper.Map<BookResponse>(book);

            return newBookResponse;
        }

        public async Task<Result> DeleteBookAsync(Guid bookId)
        {
            Book? book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book is null)
                return Result.Fail(new EntityNotFoundError($"No Book ={bookId} found"));

            if (book.IsBorrowed)
                return Result.Fail(new UnableToDeleteError($"Book ={bookId} is already borrowed"));

            await _bookRepository.DeleteBookAsync(bookId);
            return Result.Ok();
        }

        public async Task<Result> UpdateBookAsync(Guid bookId, string title, string author, string isbn)
        {
            Book? book = await _bookRepository.GetBookByIdAsync(bookId);
            if (book is null)
                return Result.Fail(new EntityNotFoundError($"No Book ={bookId} found"));

            await _bookRepository.UpdateBookAsync(bookId, title, author, isbn);
            return Result.Ok();
        }




        public async Task<Result> AddBorrowBookAsync(Guid memberId, Guid bookId)
        {
            Member? member = await _memberRepository.GetMemberByIdAsync(memberId);
            Book? book = await _bookRepository.GetBookByIdAsync(bookId);

            if (book is null)
                return Result.Fail(new EntityNotFoundError($"No Book ={bookId} found"));
            if (member is null)
                return Result.Fail(new EntityNotFoundError($"No Member ={memberId} found"));
            if (book.IsBorrowed)
                return Result.Fail(new BookUnavailableError($"Book = {bookId} is already borrowed"));
            if (member.MaxBooksAllowed == member.BorrowedBooksCount)
                return Result.Fail(new MemberReachLimitError($"Member = {memberId} has reached the borrowing limit of {member.MaxBooksAllowed} books"));


            await _memberRepository.AddBorrowedBookAsync(memberId, book);
            await _bookRepository.AddBorrowBookAsync(bookId, memberId);

            return Result.Ok();
        }


        public async Task<Result> ReturnBorrowBookAsync(Guid memberId, Guid bookId)
        {
            Member? member = await _memberRepository.GetMemberByIdAsync(memberId);
            Book? book = await _bookRepository.GetBookByIdAsync(bookId);

            if (book is null)
                return Result.Fail(new EntityNotFoundError($"No Book ={bookId} found"));
            if (member is null)
                return Result.Fail(new EntityNotFoundError($"No Member ={memberId} found"));
            if (book.BorrowerId != memberId)
                return Result.Fail(new EntityNotFoundError($"Book = {bookId} was not borrowed by Member = {memberId}"));


            await _memberRepository.RemoveBorrowedBookAsync(memberId, book);
            await _bookRepository.RemoveBorrowedBookAsync(bookId);

            return Result.Ok();

        }


    }
}
