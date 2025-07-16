using LibraryManagement.API.Datas;
using LibraryManagement.API.Profiles;
using LibraryManagement.API.Repositories;
using LibraryManagement.API.Repositories.Implementation;
using LibraryManagement.API.Repositories.Interface;
using LibraryManagement.API.Services;
using LibraryManagement.API.Services.Implementation;
using LibraryManagement.API.Services.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Output the current culture (for debugging)
Console.WriteLine(System.Globalization.CultureInfo.CurrentCulture.Name);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register Repositories
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();

// Register Services
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IBookService, BookService>();

// AutoMapper Mapping
builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddMaps(AppDomain.CurrentDomain.GetAssemblies());
});

// CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Register EF Core DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Build the app
var app = builder.Build();

// Use middleware
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
