namespace LibraryManagement.API.Datas.Models
{
    public class Book
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string ISBN { get; set; }
        public bool IsBorrowed { get; set; }
        public Guid? BorrowerId { get; set; }
        public Member? Borrower { get; set; }
        public DateTime? DateBorrowed { get; set; }
        public DateTime? DueDate { get; set; }

    }
}
