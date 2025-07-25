using LibraryManagement.API.Configurations;
using LibraryManagement.API.Datas.Models;
using LibraryManagement.API.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagement.API.Services.Implementation
{
    public class TokenService : ITokenService
    {

        public JwtSettings Settings { get; }

        public TokenService(JwtSettings settings)
        {
            Settings = settings;
        }
        public string GenerateToken(User user)
        {
            //List<Role> roles = user.Roles.Select(r => r.Name).ToList();
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Settings.Key));

            var token = new JwtSecurityToken(
                issuer: Settings.Issuer,
                audience: Settings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.Add(Settings.TokenLifeSpan),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            string tokenAsString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenAsString;

        }
    }
}
