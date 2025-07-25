using LibraryManagement.API.Datas.Models;

namespace LibraryManagement.API.Services.Interface
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}
