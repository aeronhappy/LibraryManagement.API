using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Request
{
    public class LoginRequest
    {
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
