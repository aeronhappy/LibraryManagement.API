using LibraryManagement.Application.Response;

namespace LibraryManagement.Application.Commands
{
    public interface IAuthenticationCommands
    {
        Task<AuthenticationResponse> RegisterAsync(string name, string email, string password, List<Guid> rolesId , CancellationToken cancellationToken);
        Task<AuthenticationResponse> SignInAsync(string email, string password, CancellationToken cancellationToken);
 
    }
}
