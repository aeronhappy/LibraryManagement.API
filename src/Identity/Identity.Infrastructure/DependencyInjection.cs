using Identity.Application.Queries;
using Identity.Application.Services;
using Identity.Domain.Repositories;
using Identity.Infrastructure.Data;
using Identity.Infrastructure.Data.Repositories;
using Identity.Infrastructure.QueryHandler;
using Identity.Infrastructure.Services;
using Identity.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //Register ContextDB
            services.AddDbContext<IdentityDbContext>(o =>
            {
                o.UseSqlServer(configuration
                    .GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsHistoryTable("_EFMigrationsHistory", "Identity"));
            });

            //Register JWTSETIINGS
            JwtSettings jwtSettings = new();
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
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordService, PasswordService>();

            services.AddScoped<IRoleQueries, RoleQueries>();


            return services;
        }
    }
}
