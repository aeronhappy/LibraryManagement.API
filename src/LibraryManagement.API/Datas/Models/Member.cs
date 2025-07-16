using LibraryManagement.API.DTOs.Response;

namespace LibraryManagement.API.Datas.Models
{
    public class Member
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public int MaxBooksAllowed { get; set; }
        public int BorrowedBooksCount { get; set; }
        public List<Book> BorrowedBooks { get; set; } = [];
    }
}
