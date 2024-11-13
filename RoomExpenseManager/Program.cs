using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using RoomExpenseManager.ApplicationDbContext;
using RoomExpenseManager.Implementation;
using RoomExpenseManager.Interfaces;
using RoomExpenseManager.Models;
using System.Reflection;
using RoomExpenseManager.MapperProfile;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
// Configure Serilog for logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug() // Set the minimum log level
    .WriteTo.File("logs/RoomExpenses.txt", rollingInterval: RollingInterval.Day) // Log to a file
    .CreateLogger();

// Use Serilog for logging
builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllers(); // Add MVC controllers

// Add Swagger services
builder.Services.AddEndpointsApiExplorer(); // Explore endpoints for Swagger
builder.Services.AddSwaggerGen(c =>
{
    // Set the comments path for the Swagger JSON and UI
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});
builder.Services.AddAutoMapper(typeof(MappingProfile));


// Configure Entity Framework with SQL Server
builder.Services.AddDbContext<RoomExpenseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("RoomExpenseDB"))); // Add the DbContext

// Register your services
builder.Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>(); // User repository
builder.Services.AddScoped<IGenericRepository<Expense>, GenericRepository<Expense>>(); // Expense repository
builder.Services.AddScoped<IGenericRepository<Login>, GenericRepository<Login>>();
builder.Services.AddScoped<IUserService, UserService>(); // User service
builder.Services.AddScoped<IExpenseService, ExpenseService>(); // Expense service
builder.Services.AddScoped<ILogin, LoginService>(); // Login service


var app = builder.Build();
app.UseCors("AllowAll");

// Middleware for logging
app.UseMiddleware<LoggingMiddleware>(); // Custom logging middleware

app.UseRouting(); // Ensure routing is enabled
app.UseAuthorization(); // Authorization must come after routing

// Enable Swagger middleware
app.UseSwagger(); // Enable Swagger

// Enable middleware to serve Swagger UI (HTML, JS, CSS, etc.)
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"); // Swagger endpoint
    c.RoutePrefix = "swagger"; // Set Swagger UI at the app's root
});

// Map controllers
app.MapControllers(); // Map the controllers to the app

// Run the application
app.Run(); // Start the application
