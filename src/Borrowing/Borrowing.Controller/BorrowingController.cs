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

        /// <summary>
        /// Retrieves all borrow records with optional text and date filters. (Librarian/Admin)
        /// </summary>
        /// <param name="searchText">Filter to match member name, book title/author, or other searchable fields.</param>
        /// <param name="dateTime">Optional date filter (implementation-defined, e.g., borrow date).</param>
        /// <returns>A list of borrow records.</returns>
        [Authorize(Roles = "Librarian,Admin")]
        [HttpGet("records")]
        [ProducesResponseType(typeof(List<BorrowRecordResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BorrowRecordResponse>>> GetAllRecords([FromQuery] string searchText = "", [FromQuery] DateTime? dateTime = null)
        {
            var borrowingRecords = await _borrowingQuery.GetAllBorrowRecordsAsync(searchText, dateTime);
            return Ok(borrowingRecords);
        }

        /// <summary>
        /// Retrieves unreturned (active) borrow records with optional text and date filters. (Librarian/Admin)
        /// </summary>
        /// <param name="searchText">Filter to match member name, book title/author, or other searchable fields.</param>
        /// <param name="dateTime">Optional date filter (implementation-defined, e.g., borrow date).</param>
        /// <returns>A list of unreturned borrow records.</returns>
        [Authorize(Roles = "Librarian,Admin")]
        [HttpGet("records/unreturned")]
        [ProducesResponseType(typeof(List<BorrowRecordResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BorrowRecordResponse>>> GetUnreturnedRecords([FromQuery] string searchText = "", [FromQuery] DateTime? dateTime = null)
        {
            var borrowingRecords = await _borrowingQuery.GetUnreturnedBorrowRecordsAsync(searchText, dateTime);
            return Ok(borrowingRecords);
        }

        /// <summary>
        /// Retrieves returned (completed) borrow records with optional text and date filters. (Librarian/Admin)
        /// </summary>
        /// <param name="searchText">Filter to match member name, book title/author, or other searchable fields.</param>
        /// <param name="dateTime">Optional date filter (implementation-defined, e.g., return date).</param>
        /// <returns>A list of returned borrow records.</returns>
        [Authorize(Roles = "Librarian,Admin")]
        [HttpGet("records/returned")]
        [ProducesResponseType(typeof(List<BorrowRecordResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BorrowRecordResponse>>> GetReturnedRecords([FromQuery] string searchText = "", [FromQuery] DateTime? dateTime = null)
        {
            var borrowingRecords = await _borrowingQuery.GetReturnedBorrowRecordsAsync(searchText, dateTime);
            return Ok(borrowingRecords);
        }

        /// <summary>
        /// Retrieves all borrow requests awaiting action, optionally filtered by search text. (Librarian/Admin)
        /// </summary>
        /// <param name="searchText">Filter to match member name, book title/author, or other searchable fields.</param>
        /// <returns>A list of borrow requests.</returns>
        [Authorize(Roles = "Librarian,Admin")]
        [HttpGet("requestBorrow")]
        [ProducesResponseType(typeof(List<BorrowRequestResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<List<BorrowRequestResponse>>> GetAllBorrowRequest([FromQuery] string searchText = "")
        {
            var borrowingRequest = await _borrowingQuery.GetBorrowRequestAsync(searchText);
            return Ok(borrowingRequest);
        }

        /// <summary>
        /// Creates a borrow request for the authenticated member for the specified book. (Member)
        /// </summary>
        /// <param name="bookId">The ID of the book to request.</param>
        /// <returns>200 if created; appropriate error status otherwise.</returns>
        [Authorize(Roles = "Member")]
        [HttpPost("{bookId}/requestBorrow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // EntityNotFoundError
        [ProducesResponseType(StatusCodes.Status409Conflict)]  // BookUnavailableError
        [ProducesResponseType(StatusCodes.Status400BadRequest)]// MemberReachLimitError
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RequestBorrowBook(Guid bookId)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
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

        /// <summary>
        /// Accepts a borrow request (approves it for fulfillment). (Librarian/Admin)
        /// </summary>
        /// <param name="requestBorrowId">The ID of the borrow request to accept.</param>
        /// <returns>200 if accepted; appropriate error status otherwise.</returns>
        [Authorize(Roles = "Librarian,Admin")]
        [HttpPost("{requestBorrowId}/accept")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // EntityNotFoundError
        [ProducesResponseType(StatusCodes.Status409Conflict)]  // BookUnavailableError
        [ProducesResponseType(StatusCodes.Status400BadRequest)]// MemberReachLimitError
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AcceptRequestBorrow(Guid requestBorrowId)
        {
            // Note: userId is currently unused; keep if you plan to audit who accepted.
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

        /// <summary>
        /// Borrows a book on behalf of a member. (Librarian/Admin)
        /// </summary>
        /// <param name="bookId">The ID of the book to borrow.</param>
        /// <param name="borrowerId">The ID of the member borrowing the book.</param>
        /// <returns>200 if borrowed; appropriate error status otherwise.</returns>
        [Authorize(Roles = "Librarian,Admin")]
        [HttpPost("{bookId}/borrow")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // EntityNotFoundError
        [ProducesResponseType(StatusCodes.Status409Conflict)]  // BookUnavailableError
        [ProducesResponseType(StatusCodes.Status400BadRequest)]// MemberReachLimitError
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        /// <summary>
        /// Returns a borrowed book on behalf of a member. (Librarian/Admin)
        /// </summary>
        /// <param name="bookId">The ID of the book to return.</param>
        /// <param name="borrowerId">The ID of the member returning the book.</param>
        /// <returns>200 if returned; appropriate error status otherwise.</returns>
        [Authorize(Roles = "Librarian,Admin")]
        [HttpPost("{bookId}/return")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]  // EntityNotFoundError
        [ProducesResponseType(StatusCodes.Status409Conflict)]  // BookAlreadyReturnedError
        [ProducesResponseType(StatusCodes.Status400BadRequest)]// MemberCantReturnWithoutBorrowingError
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
    }
}
