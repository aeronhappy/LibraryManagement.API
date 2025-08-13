using Borrowing.Application.Commands;
using Borrowing.Application.Errors;
using Borrowing.Application.Queries;
using Borrowing.Application.Response;
using Borrowing.Controller.Request;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Borrowing.Controller
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookQueries _bookService;
        private readonly IBookCommands _bookCommand;

        public BooksController(IBookQueries bookService, IBookCommands bookCommand)
        {
            _bookService = bookService;
            _bookCommand = bookCommand;
        }

        /// <summary>
        /// Retrieves all books, optionally filtered by search text. (All User)
        /// </summary>
        /// <param name="searchText">Search filter to match book title or author.</param>
        /// <returns>A list of books.</returns>
        [HttpGet()]
        public async Task<ActionResult<List<BookResponse>>> GetListOfBook([FromQuery] string searchText = "")
        {
            var listOfBooks = await _bookService.GetAllBooksAsync(searchText);
            return Ok(listOfBooks);
        }

        /// <summary>
        /// Retrieves all available books (not currently borrowed), optionally filtered by search text.  (All User)
        /// </summary>
        /// <param name="searchText">Search filter to match book title or author.</param>
        /// <returns>A list of available books.</returns>
        [HttpGet("available")]
        public async Task<ActionResult<List<BookResponse>>> GetAllAvailableBooks([FromQuery] string searchText = "")
        {
            var listOfBooks = await _bookService.GetAllAvailableBooksAsync(searchText);
            return Ok(listOfBooks);
        }

        /// <summary>
        /// Retrieves a list of overdue books (not returned past their due date), optionally filtered by search text. (All User)
        /// </summary>
        /// <param name="searchText">Search filter to match book title or author.</param>
        /// <returns>A list of overdue books.</returns>
        [HttpGet("overdue")]
        public async Task<ActionResult<List<BookResponse>>> GetOverdueBooks([FromQuery] string searchText = "")
        {
            var listOfBooks = await _bookService.GetOverdueBooksAsync(searchText);
            return Ok(listOfBooks);
        }

        /// <summary>
        /// Retrieves a book by its unique identifier. (All User)
        /// </summary>
        /// <param name="bookId">The ID of the book to retrieve.</param>
        /// <returns>The requested book, or 404 if not found.</returns>
        [HttpGet("{bookId}")]
        public async Task<ActionResult<BookResponse>> GetBooksById(Guid bookId)
        {
            var bookResponse = await _bookService.GetBookByIdAsync(bookId);
            if (bookResponse is null)
                return NotFound();

            return Ok(bookResponse);
        }

        /// <summary>
        /// Adds a new book to the system. (Librarian/Admin) 
        /// </summary>
        /// <param name="addNewBook">The details of the new book to add.</param>
        /// <returns>The created book.</returns>
        [Authorize(Roles = "Librarian,Admin")]
        [HttpPost("create")]
        public async Task<ActionResult<BookResponse>> AddNewBook(AddBookRequest addNewBook)
        {
            var bookResponse = await _bookCommand.AddBookAsync(
                title: addNewBook.Title,
                author: addNewBook.Author,
                isbn: addNewBook.ISBN,
                HttpContext.RequestAborted
            );

            return Ok(bookResponse.Value);
        }

        /// <summary>
        /// Deletes an existing book by its unique identifier. (Librarian/Admin) 
        /// </summary>
        /// <param name="bookId">The ID of the book to delete.</param>
        /// <response code="404">If book not found</response>

        [Authorize(Roles = "Librarian,Admin")]
        [HttpDelete("{bookId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteBook(Guid bookId)
        {
            Result result = await _bookCommand.DeleteBookAsync(bookId, HttpContext.RequestAborted);
            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);
            else if (result.HasError<UnableToDeleteError>(out var unableToDeleteErrors))
                return BadRequest(unableToDeleteErrors.FirstOrDefault()?.Message);

            return Ok();
        }

        /// <summary>
        /// Updates an existing book's details. (Librarian/Admin) 
        /// </summary>
        /// <param name="bookId">The ID of the book to update.</param>
        /// <param name="newBook">The updated book details.</param>
        [Authorize(Roles = "Librarian,Admin")]
        [HttpPut("{bookId}")]
        public async Task<ActionResult> UpdateBook(Guid bookId, AddBookRequest newBook)
        {
            Result result = await _bookCommand.UpdateBookAsync(bookId, newBook.Title, newBook.Author, newBook.ISBN, HttpContext.RequestAborted);
            if (result.HasError<EntityNotFoundError>(out var errors))
                return NotFound(errors.FirstOrDefault()?.Message);

            return Ok();
        }
    }
}
