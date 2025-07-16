namespace LibraryManagement.API.DTOs.Response
{
    public class BookResponse
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string ISBN { get; set; }
        public bool IsBorrowed { get; set; }
        public BorrowerResponse? Borrower { get; set; }
        public DateTime? DateBorrowed { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
