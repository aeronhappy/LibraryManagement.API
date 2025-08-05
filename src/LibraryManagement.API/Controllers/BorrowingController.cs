using FluentResults;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Errors;
using LibraryManagement.Application.Response;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.API.Controllers
{
    [Route("api/borrowing")]
    [ApiController]
    public class BorrowingController : ControllerBase
    {
        private readonly IBorrowingQueryService _borrowingQuery;
        private readonly IBorrowingCommandService _borrowingCommand;

        public BorrowingController(IBorrowingQueryService borrowingQuery, IBorrowingCommandService borrowingCommand)
        {
            _borrowingQuery = borrowingQuery;
            _borrowingCommand = borrowingCommand;
        }

        [HttpPost("books/{bookId}")]
        public async Task<ActionResult> BorrowBook(Guid bookId, [FromQuery] Guid borrowerId)
        {
            Result result = await _borrowingCommand.BorrowAsync(bookId, borrowerId, HttpContext.RequestAborted);
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
            Result result = await _borrowingCommand.ReturnAsync(bookId, borrowerId, HttpContext.RequestAborted);
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


        [HttpGet("records")]
        public async Task<ActionResult<List<BorrowingRecord>>> GetListOfBorrowingRecords([FromQuery] string searchText = "", [FromQuery] DateTime? dateTime = null)
        {
            var borrowingRecords = await _borrowingQuery.GetAllBorrowingRecord(searchText ,dateTime);
            return Ok(borrowingRecords);
        }


    }
}
