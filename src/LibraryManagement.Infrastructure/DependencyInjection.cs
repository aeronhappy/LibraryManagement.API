using LibraryManagement.API.Repositories;
using LibraryManagement.Application.Services;
using LibraryManagement.Domain.Repositories;
using LibraryManagement.Infrastructure.Configurations;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.Data.Repositories;
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
            JwtSettings jwtSettings = new JwtSettings();
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


            // Register Repositories
           services.AddScoped<IMemberRepository, MemberRepository>();
           services.AddScoped<IBookRepository, BookRepository>();
           services.AddScoped<IUserRepository, UserRepository>();
           services.AddScoped<IRoleRepository, RoleRepository>();
           services.AddScoped<IBorrowingRecordRepository, BorrowingRecordRepository>();
           services.AddScoped<IUnitOfWork, UnitOfWork>();



            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordService, PasswordService>();


            return services;
        }
    }
}
