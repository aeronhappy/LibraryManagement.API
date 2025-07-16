namespace LibraryManagement.API.DTOs.Request
{
    public class AddNewBook
    {
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string ISBN { get; set; }

    }
}
