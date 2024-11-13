using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;



public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log the request details
        context.Request.EnableBuffering();
        var requestBodyStream = new StreamReader(context.Request.Body);
        var requestBody = await requestBodyStream.ReadToEndAsync();
        context.Request.Body.Position = 0; // Reset the stream position for the next middleware

        _logger.LogInformation($"Request: {context.Request.Method} {context.Request.Path} - Body: {requestBody}");

        // Capture the original response body stream
        var originalResponseBodyStream = context.Response.Body;

        using (var responseBodyStream = new MemoryStream())
        {
            context.Response.Body = responseBodyStream; // Use the memory stream for the response body

            await _next(context); // Call the next middleware in the pipeline

            // Reset the response body stream position
            responseBodyStream.Position = 0;

            var responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            _logger.LogInformation($"Response: {context.Response.StatusCode} - Body: {responseBody}");

            // Reset the position again to write to the original stream
            responseBodyStream.Position = 0;
            await responseBodyStream.CopyToAsync(originalResponseBodyStream); // Copy the memory stream to the original response stream
        }
    }
}
