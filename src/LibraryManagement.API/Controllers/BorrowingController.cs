using FluentResults;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/borrowing")]
    [ApiController]
    public class BorrowingController : ControllerBase
    {
        private readonly IBorrowingCommandService _borrowingService;

        public BorrowingController(IBorrowingCommandService borrowingCommandService)
        {
            _borrowingService = borrowingCommandService;
        }

        [HttpPost("books/{bookId}")]
        public async Task<ActionResult> BorrowBook(Guid bookId, [FromQuery] Guid borrowerId)
        {
            Result result = await _borrowingService.BorrowAsync(bookId, borrowerId, HttpContext.RequestAborted);
            if (result.IsFailed)
            {
                var error = result.Errors.First();

                return error switch
                {
                    EntityNotFoundError => NotFound(error.Message),
                    BookUnavailableError => Conflict(error.Message),
                    MemberReachLimitError => BadRequest(error.Message),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Message)
                };
            }

            return Ok();
        }

        [HttpPost("books/{bookId}/return")]
        public async Task<ActionResult> ReturnBook(Guid bookId, [FromQuery] Guid borrowerId)
        {
            Result result = await _borrowingService.ReturnAsync(bookId, borrowerId, HttpContext.RequestAborted);
            if (result.IsFailed)
            {
                var error = result.Errors.First();

                return error switch
                {
                    EntityNotFoundError => NotFound(error.Message),
                    BookAlreadyReturnedError => Conflict(error.Message),
                    MemberCantReturnWithoutBorrowingError => BadRequest(error.Message),
                    _ => StatusCode(StatusCodes.Status500InternalServerError, error.Message)
                };
            }

            return Ok();
        }

   
    }
}
