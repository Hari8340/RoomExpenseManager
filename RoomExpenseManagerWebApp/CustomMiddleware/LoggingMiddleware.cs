using Serilog;
using System.Diagnostics;

namespace RoomExpenseManagerWebApp.CustomMiddleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log the entering of a controller and method
            var stopwatch = Stopwatch.StartNew();
            var controllerName = context.Request.RouteValues["controller"]?.ToString();
            var actionName = context.Request.RouteValues["action"]?.ToString();

            Log.Information("Entering {Controller}.{Action}", controllerName, actionName);

            // Log request details
            var requestBody = context.Request.Body;
            var requestMethod = context.Request.Method;
            var requestPath = context.Request.Path;

            // Read the request body
            context.Request.EnableBuffering();
            using (var reader = new StreamReader(requestBody, System.Text.Encoding.UTF8, leaveOpen: true))
            {
                var requestBodyContent = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; // Reset the stream position for the next middleware
                Log.Information("Request: {Method} {Path} {Body}", requestMethod, requestPath, requestBodyContent);
            }

            try
            {
                await _next(context); // Call the next middleware in the pipeline

                // Log response details
                var responseStatusCode = context.Response.StatusCode;
                Log.Information("Response: {StatusCode}", responseStatusCode);
            }
            catch (Exception ex)
            {
                // Log exception details
                Log.Error(ex, "Exception occurred in {Controller}.{Action}", controllerName, actionName);
                throw; // Re-throw the exception after logging
            }
            finally
            {
                stopwatch.Stop();
                Log.Information("Exiting {Controller}.{Action} in {Elapsed} ms", controllerName, actionName, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
