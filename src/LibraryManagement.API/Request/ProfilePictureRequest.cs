using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.Request
{
    public class ProfilePictureRequest
    {
        public required IFormFile Image { get; set; }
      
    }
}
