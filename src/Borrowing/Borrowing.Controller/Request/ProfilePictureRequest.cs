using Microsoft.AspNetCore.Http;

namespace Borrowing.Controller.Request
{
    public class ProfilePictureRequest
    {
        public required IFormFile Image { get; set; }

    }
}
