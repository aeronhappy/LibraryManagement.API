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
            services.AddScoped<IBookCommandSerice, BookCommandService>();
            services.AddScoped<IMemberCommandService, MemberCommandService>();
            services.AddScoped<IAuthenticationCommandService, AuthenticationCommandService>();
            services.AddScoped<IRoleCommandService, RoleCommandService>();
            services.AddScoped<IBorrowingCommandService, BorrowingCommandService>();


            return services;
        }
    }
}
