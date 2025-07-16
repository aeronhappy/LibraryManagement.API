using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.DTOs.Request
{
    public class CreateMemberRequest
    {
        public required string Name { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
    }
}
