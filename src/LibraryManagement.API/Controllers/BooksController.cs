using FluentResults;
using LibraryManagement.API.DTOs.Request;
using LibraryManagement.API.DTOs.Response;
using LibraryManagement.API.Errors;
using LibraryManagement.API.Services;
using LibraryManagement.API.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet()]
        public async Task<ActionResult<List<BookResponse>>> GetListOfBook([FromQuery]string searchText = "")
        {
            var listOfBooks = await _bookService.GetAllBooksAsync(searchText);
            return Ok(listOfBooks);
        }

        [HttpGet("available")]
        public async Task<ActionResult<List<BookResponse>>> GetAllAvailableBooks([FromQuery] string searchText = "")
        {
            var listOfBooks = await _bookService.GetAllAvailableBooksAsync(searchText);
            return Ok(listOfBooks);
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<List<BookResponse>>> GetOverdueBooks([FromQuery] string searchText = "")
        {
            var listOfBooks = await _bookService.GetOverdueBooksAsync(searchText);
            return Ok(listOfBooks);
        }


        [HttpGet("{bookId}")]
        public async Task<ActionResult<BookResponse>> GetBooksById(Guid bookId)
        {
            var bookResponse = await _bookService.GetBookByIdAsync(bookId);
            if (bookResponse is null)
                return NotFound();

            return Ok(bookResponse);
        }

        [HttpPost("create")]
        public async Task<ActionResult<BookResponse>> AddNewBook(AddNewBook addNewBook)
        {
            var bookResponse = await _bookService.AddBookAsync(title: addNewBook.Title, author: addNewBook.Author, isbn: addNewBook.ISBN);
            return Ok(bookResponse.Value);
        }


        [HttpDelete("{bookId}")]
        public async Task<ActionResult> DeleteBook(Guid bookId)
        {
            Result result = await _bookService.DeleteBookAsync(bookId);
            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);
            else if (result.HasError<UnableToDeleteError>(out var unableToDeleteErrors))
                return BadRequest(unableToDeleteErrors.FirstOrDefault()?.Message);


            return Ok();
        }

        [HttpPut("{bookId}")]
        public async Task<ActionResult> UpdateBook(Guid bookId, AddNewBook newBook)
        {
            Result result = await _bookService.UpdateBookAsync(bookId, newBook.Title, newBook.Author, newBook.ISBN);
            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);

            return Ok();
        }


        [HttpPost("{bookId}/borrow")]
        public async Task<ActionResult> BorrowBook( Guid bookId, [FromQuery]Guid memberId)
        {

            Result result = await _bookService.AddBorrowBookAsync(memberId, bookId);

            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);
            else if (result.HasError<MemberReachLimitError>(out var reachLimitErrors))
                return BadRequest(reachLimitErrors.FirstOrDefault()?.Message);
            else if (result.HasError<BookUnavailableError>(out var bookUnavailableErrors))
                return BadRequest(bookUnavailableErrors.FirstOrDefault()?.Message);

            return Ok();

        }

        [HttpPost("{bookId}/return")]
        public async Task<ActionResult> ReturnBook( Guid bookId, [FromQuery] Guid memberId)
        {

            Result result = await _bookService.ReturnBorrowBookAsync(memberId, bookId);

            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);

            return Ok();

        }

    }
}
