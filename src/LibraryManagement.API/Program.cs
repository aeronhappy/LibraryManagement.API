
using Borrowing.Application;
using Borrowing.Controller;
using Borrowing.Infrastructure;
using Identity.Application;
using Identity.Controller;
using Identity.Infrastructure;
using LibraryManagement.Contracts.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);


Console.WriteLine(System.Globalization.CultureInfo.CurrentCulture.Name);

// Add services to the container
builder.Services.AddControllers()
    .AddApplicationPart(typeof(AuthenticationController).Assembly)
    .AddApplicationPart(typeof(BooksController).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer Token"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


//Register Mapping Profile
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Borrowing.Application.AssemblyReference).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(Identity.Application.AssemblyReference).Assembly);
});



//Register Dependency Injection

//Identity
builder.Services.AddIdentityInfrastructure(builder.Configuration);
builder.Services.AddIdentityApplication();

//Borrowing
builder.Services.AddBorrowingInfrastructure(builder.Configuration);
builder.Services.AddBorrowingApplication();

builder.Services.AddScoped<IEventBus, EventBus>();
builder.Services.AddScoped<IDomainEventPublisher, DomainEventPublisher>();


// CORS
//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});


var app = builder.Build();

// Use middleware
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
