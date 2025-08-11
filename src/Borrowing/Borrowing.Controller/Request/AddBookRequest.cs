namespace Borrowing.Controller.Request
{
    public class AddBookRequest
    {
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string ISBN { get; set; }

    }
}
