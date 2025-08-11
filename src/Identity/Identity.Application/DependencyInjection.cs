using Identity.Application.CommandHandler;
using Identity.Application.Commands;
using Microsoft.Extensions.DependencyInjection;


namespace Identity.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddIdentityApplication(this IServiceCollection services)
        {
            // Register Services
            services.AddScoped<IAuthenticationCommands, AuthenticationCommands>();
            services.AddScoped<IRoleCommands, RoleCommandSs>();

            return services;
        }
    }
}
