using AutoMapper;
using LibraryManagement.Application.Queries;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Domain.ValueObjects;

namespace LibraryManagement.Application.QueryHandler
{
    public class BookQueryService : IBookQueryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public BookQueryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<List<BookResponse>> GetAllBooksAsync(string searchText)
        {
            List<Book> listOfBooks = await _unitOfWork.Books.GetAllBooksAsync();

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
            List<Book> listOfBooks = await _unitOfWork.Books.GetAllBooksAsync();

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
            List<Book> listOfBooks = await _unitOfWork.Books.GetAllBooksAsync();

            DateTime now = DateTime.Now;
            listOfBooks = listOfBooks.Where(book => now >= book.DateOverdue).ToList();

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
            Book? book = await _unitOfWork.Books.GetBookByIdAsync(new BookId(bookId));
            if (book is null)
                return null;

            var newBookResponse = _mapper.Map<BookResponse>(book);

            return newBookResponse;
        }





    }
}
