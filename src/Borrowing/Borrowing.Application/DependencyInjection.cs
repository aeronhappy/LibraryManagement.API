

using Borrowing.Application.CommandHandler;
using Borrowing.Application.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace Borrowing.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddBorrowingApplication(this IServiceCollection services)
        {
            // Register Services
            services.AddScoped<IBookCommands, BookCommands>();
            services.AddScoped<IMemberCommands, MemberCommands>();
            services.AddScoped<IBorrowingCommands, BorrowingCommands>();



            return services;
        }
    }
}
