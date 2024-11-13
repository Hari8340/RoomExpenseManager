using RoomExpenseManagerWebApp.CustomMiddleware;
using RoomExpenseManagerWebApp.Services.Implementation.Expense;
using RoomExpenseManagerWebApp.Services.Implementation.Login;
using RoomExpenseManagerWebApp.Services.Implementation.User;
using RoomExpenseManagerWebApp.Services.Interface.IExpense;
using RoomExpenseManagerWebApp.Services.Interface.ILogin;
using RoomExpenseManagerWebApp.Services.Interface.IUser;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Set up Serilog configuration with rolling file logs
//var logFilePath = "logs/log-.txt"; // Specify the log file path and format
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Information() // Log only Information level and higher
//    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day) // Daily rolling logs
//    .Enrich.FromLogContext()
//    .CreateLogger();

// Use Serilog as the logging provider
//builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); // Optional: If external HTTP calls are needed
builder.Services.AddScoped<IUser, User>(); // Dependency Injection for User Service
builder.Services.AddScoped<IExpense, Expense>(); // Dependency Injection for Expense Service
builder.Services.AddScoped<ILogin, Login>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);  // Session timeout
    options.Cookie.HttpOnly = true;  // Prevent JavaScript access to cookies
    options.Cookie.IsEssential = true;  // Required for GDPR compliance
});

//builder.WebHost.UseUrls("https://192.168.1.12:44395", "https://192.168.1.12:44395");
var app = builder.Build();

// Use custom logging middleware
//app.UseMiddleware<LoggingMiddleware>();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Handle exceptions in non-development mode
    app.UseHsts(); // Enforce HTTPS with HSTS
}

// Ensure requests are redirected to HTTPS
app.UseHttpsRedirection();
app.UseStaticFiles(); // Serve static files (like CSS, JS, images)

// Set up request routing
app.UseRouting();
// Add session middleware here
app.UseSession();  // Add this line


// Optional: Add authentication/authorization if needed
app.UseAuthorization();

// Map controller routes with a default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}/{id?}");

// Run the application
app.Run();
