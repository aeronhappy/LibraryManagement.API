using FluentResults;
using LibraryManagement.API.Request;
using LibraryManagement.API.Services.Interface;
using LibraryManagement.Application.Errors;
using LibraryManagement.Application.Response;
using LibraryManagement.Infrastructure.Queries;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookQueryService _bookService;
        private readonly IBookCommandSerice _bookCommand;

        public BooksController(IBookQueryService bookService, IBookCommandSerice bookCommand)
        {
            _bookService = bookService;
            _bookCommand = bookCommand;
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
        public async Task<ActionResult<BookResponse>> AddNewBook(AddBookRequest addNewBook)
        {
            var bookResponse = await _bookCommand.AddBookAsync(title: addNewBook.Title, author: addNewBook.Author, isbn: addNewBook.ISBN, HttpContext.RequestAborted);
            return Ok(bookResponse.Value);
        }


        [HttpDelete("{bookId}")]
        public async Task<ActionResult> DeleteBook(Guid bookId)
        {
            Result result = await _bookCommand.DeleteBookAsync(bookId, HttpContext.RequestAborted);
            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);
            else if (result.HasError<UnableToDeleteError>(out var unableToDeleteErrors))
                return BadRequest(unableToDeleteErrors.FirstOrDefault()?.Message);


            return Ok();
        }

        [HttpPut("{bookId}")]
        public async Task<ActionResult> UpdateBook(Guid bookId, AddBookRequest newBook)
        {
            Result result = await _bookCommand.UpdateBookAsync(bookId, newBook.Title, newBook.Author, newBook.ISBN , HttpContext.RequestAborted);
            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);

            return Ok();
        }


      

    }
}
