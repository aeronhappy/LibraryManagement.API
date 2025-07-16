namespace LibraryManagement.API.DTOs.Response
{
    public class MemberResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public int MaxBooksAllowed { get; set; }
        public int BorrowedBooksCount { get; set; }
        public List<BookResponse> BorrowedBooks { get; set; } = [];
    }
}
