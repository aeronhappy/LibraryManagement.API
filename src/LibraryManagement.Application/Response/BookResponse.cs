namespace LibraryManagement.Application.Response
{
    public class BookResponse
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string ISBN { get; set; }
        public bool IsBorrowed { get; set; }
        public List<MemberBorrowRecordResponse> BorrowingHistory { get; set; } = [];

    }

    public class BookBorrowedResponse
    {
        public Guid Id { get; set; } 
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string ISBN { get; set; }
        public bool IsBorrowed { get; set; }

    }


}
