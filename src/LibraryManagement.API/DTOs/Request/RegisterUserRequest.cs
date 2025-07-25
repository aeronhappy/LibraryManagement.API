using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.DTOs.Response;
using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.DTOs.Request
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
