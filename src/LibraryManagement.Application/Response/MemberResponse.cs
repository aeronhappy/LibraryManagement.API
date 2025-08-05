namespace LibraryManagement.Application.Response
{
    public class MemberResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public int MaxBooksAllowed { get; set; }
        public int BorrowedBooksCount { get; set; }
        public List<BookBorrowRecordResponse> BorrowingHistory { get; set; } = [];
    }


    public class BorrowerResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public int MaxBooksAllowed { get; set; }
        public int BorrowedBooksCount { get; set; }
    }
}
