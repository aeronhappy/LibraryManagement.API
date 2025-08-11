using Identity.Domain.Entities;

namespace Identity.Application.Services
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
