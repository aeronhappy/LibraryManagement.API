using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Request
{
    public class RegisterUserRequest
    {
        public required string Name { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public List<Guid> RolesId { get; set; } = [];
    }
}
