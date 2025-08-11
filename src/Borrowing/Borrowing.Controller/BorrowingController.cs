using Borrowing.Application.Commands;
using Borrowing.Application.Errors;
using Borrowing.Application.Queries;
using Borrowing.Application.Response;
using Borrowing.Domain.Entities;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Borrowing.Controller
{
    [Route("api/borrowings")]
    [ApiController]
    public class BorrowingController : ControllerBase
    {
        private readonly IBorrowingQueries _borrowingQuery;
        private readonly IBorrowingCommands _borrowingCommand;

        public BorrowingController(IBorrowingQueries borrowingQuery, IBorrowingCommands borrowingCommand)
        {
            _borrowingQuery = borrowingQuery;
            _borrowingCommand = borrowingCommand;
        }


        [Authorize(Roles = "Member")]
        [HttpPost("{bookId}/requestBorrow")]
        public async Task<ActionResult> RequestBorrowBook(Guid bookId)
        {

            var userId= User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?
                .Value;
            var newGuid = Guid.Parse(userId!);

            Result result = await _borrowingCommand.BorrowRequestAsync(bookId, newGuid, HttpContext.RequestAborted);
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


        [Authorize(Roles = "Librarian")]
        [HttpPost("{requestBorrowId}/accept")]
        public async Task<ActionResult> AcceptRequestBorrow(Guid requestBorrowId)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var newGuid = Guid.Parse(userId!);

            Result result = await _borrowingCommand.AcceptRequestAsync(requestBorrowId, HttpContext.RequestAborted);
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

        [HttpPost("{bookId}/borrow")]
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




        [HttpPost("{bookId}/return")]
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
        public async Task<ActionResult<List<BorrowRecordResponse>>> GetAllRecords([FromQuery] string searchText = "", [FromQuery] DateTime? dateTime = null)
        {
            var borrowingRecords = await _borrowingQuery.GetAllBorrowRecordsAsync(searchText, dateTime);
            return Ok(borrowingRecords);
        }

        [HttpGet("records/unreturned")]
        public async Task<ActionResult<List<BorrowRecordResponse>>> GetUnreturnedRecords([FromQuery] string searchText = "", [FromQuery] DateTime? dateTime = null)
        {
            var borrowingRecords = await _borrowingQuery.GetUnreturnedBorrowRecordsAsync(searchText, dateTime);
            return Ok(borrowingRecords);
        }

        [HttpGet("records/returned")]
        public async Task<ActionResult<List<BorrowRecordResponse>>> GetReturnedRecords([FromQuery] string searchText = "", [FromQuery] DateTime? dateTime = null)
        {
            var borrowingRecords = await _borrowingQuery.GetReturnedBorrowRecordsAsync(searchText, dateTime);
            return Ok(borrowingRecords);
        }


    }
}
