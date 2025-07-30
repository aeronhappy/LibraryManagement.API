using LibraryManagement.Application.Services;
using System.Text;

namespace LibraryManagement.Infrastructure.Services
{
    public class PasswordService : IPasswordService

    {
        public string HashPassword(string email, string password)
        {
            var salt = Encoding.UTF8.GetBytes(email.ToLowerInvariant());

            using (var hmac = new System.Security.Cryptography.HMACSHA256(salt))
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashBytes = hmac.ComputeHash(passwordBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        public bool ValidatePassword(string email, string password, string passwordHash)
        {
            var hash = HashPassword(email, password);
            return hash == passwordHash;
        }
    }
}
