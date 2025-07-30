using LibraryManagement.Application.Response;

namespace LibraryManagement.Application.Commands
{
    public interface IAuthenticationCommandService
    {
        Task<AuthenticationResponse> RegisterAsync(string name, string email, string password, List<Guid> rolesId);
        Task<AuthenticationResponse> SignInAsync(string email, string password);
        Task<UserResponse?> GetUserByIdAsync(Guid id);
        Task<List<UserResponse>> GetAllUsersAsync();
    }
}
