using LibraryManagement.API.Repositories;
using LibraryManagement.Application.CommandHandler;
using LibraryManagement.Application.Commands;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Infrastructure.Configurations;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Data.Repositories;
using LibraryManagement.Infrastructure.MappingProfile;
using LibraryManagement.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            //Register ContextDB
            services.AddDbContext<ApplicationDbContext>(o =>
            {
                o.UseSqlServer(configuration
                    .GetConnectionString("DefaultConnection"));
            });

            //Register JWTSETIINGS
            var jwtSettings = new JwtSettings
            {
                Issuer = configuration["JwtSettings:Issuer"] ?? string.Empty,
                Audience = configuration["JwtSettings:Audience"] ?? string.Empty,
                Key = configuration["JwtSettings:Key"] ?? string.Empty
            };
           configuration.Bind("JwtSettings", jwtSettings);
           services.AddSingleton(jwtSettings);
            services.AddAuthentication("Bearer")
                 .AddJwtBearer("Bearer", options =>
                 {
                     TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidateIssuerSigningKey = true,
                         ValidIssuer = jwtSettings.Issuer,
                         ValidAudience = jwtSettings.Audience,
                         IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(jwtSettings.Key))
                     };

                     options.TokenValidationParameters = tokenValidationParameters;
                 });

            //Register Mapping Profile
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register Repositories
           services.AddScoped<IMemberRepository, MemberRepository>();
           services.AddScoped<IBookRepository, BookRepository>();
           services.AddScoped<IUserRepository, UserRepository>();
           services.AddScoped<IRoleRepository, RoleRepository>();
           services.AddScoped<IBorrowingRecordRepository, BorrowingRecordRepository>();



            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordService, PasswordService>();


            return services;
        }
    }
}
