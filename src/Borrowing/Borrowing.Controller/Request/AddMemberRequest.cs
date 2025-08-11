using System.ComponentModel.DataAnnotations;

namespace Borrowing.Controller.Request
{
    public class AddMemberRequest
    {
        public required string Name { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
    }
}
