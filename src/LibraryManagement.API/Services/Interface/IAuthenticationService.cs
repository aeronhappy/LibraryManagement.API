
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.DTOs.Response;

namespace LibraryManagement.API.Services.Interface
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> RegisterAsync(string name, string email, string password, List<Guid> rolesId);
        Task<AuthenticationResponse> SignInAsync(string email, string password);
        Task<UserResponse?> GetUserByIdAsync(Guid id);
        Task<List<UserResponse>> GetAllUsersAsync();
    }
}
