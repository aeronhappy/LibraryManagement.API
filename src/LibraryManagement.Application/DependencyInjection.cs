using LibraryManagement.API.Services.Interface;
using LibraryManagement.Application.CommandHandler;
using LibraryManagement.Application.Commands;
using Microsoft.Extensions.DependencyInjection;


namespace LibraryManagement.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register Services
            services.AddScoped<IBookCommands, BookCommands>();
            services.AddScoped<IMemberCommands, MemberCommands>();
            services.AddScoped<IAuthenticationCommands, AuthenticationCommands>();
            services.AddScoped<IRoleCommands, RoleCommandSs>();
            services.AddScoped<IBorrowingCommands, BorrowingCommands>();

            return services;
        }
    }
}
