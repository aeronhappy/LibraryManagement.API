using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.DTOs.Request
{
    public class AddMemberRequest
    {
        public required string Name { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
    }
}
