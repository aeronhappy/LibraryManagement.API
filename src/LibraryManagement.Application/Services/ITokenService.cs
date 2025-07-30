using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
