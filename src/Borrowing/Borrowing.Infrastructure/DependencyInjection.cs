using Borrowing.Application.Queries;
using Borrowing.Domain.Repositories;
using Borrowing.Infrastructure.Data;
using Borrowing.Infrastructure.Data.Repositories;
using Borrowing.Infrastructure.QueryHandler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Borrowing.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddBorrowingInfrastructure(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            //Register ContextDB
            services.AddDbContext<BorrowingDbContext>(o =>
            {
                o.UseSqlServer(configuration
                    .GetConnectionString("DefaultConnection"),
                    sql => sql.MigrationsHistoryTable("_EFMigrationsHistory", "Borrowing"));
            });



            // Register Repositories
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<IBookRepository, BookRepository>();
            services.AddScoped<IBorrowingRecordRepository, BorrowingRecordRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();



            services.AddScoped<IBookQueries, BookQueries>();
            services.AddScoped<IMemberQueries, MemberQueries>();
            services.AddScoped<IBorrowingQueries, BorrowingQueries>();



            return services;
        }
    }
}
