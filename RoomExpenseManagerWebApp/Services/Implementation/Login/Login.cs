using RoomExpenseManagerWebApp.Constants;
using RoomExpenseManagerWebApp.Services.Interface.ILogin;
using Serilog;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace RoomExpenseManagerWebApp.Services.Implementation.Login
{
    public class Login:ILogin
    {
        private readonly HttpClient _httpClient;

        public Login(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<JsonObject> GetUserByCred(Models.Login login)
        {
            try
            {
                //var requestUri = $"https://localhost:44374/api/Login/GetUserLoginById?userId={login.Id}&password={login.Password}";
                var requestUri = $"http://localhost/api/Login/GetUserLoginById?userId={login.Id}&password={login.Password}";
                Log.Information("Sending request to: {RequestUri}", requestUri);

                var response = await _httpClient.GetAsync(requestUri);
                var responseContent = await response.Content.ReadAsStringAsync();
                Log.Information("Received response: {ResponseContent}", responseContent);

                if (response.IsSuccessStatusCode)
                {
                    // Successful login response
                    var jsonObject = JsonSerializer.Deserialize<JsonObject>(responseContent);
                    return new JsonObject
            {
                { "isSuccess", true },
                { "message", "Login successful." }, // Indicating success
                { "data", jsonObject } // Include any user data if necessary
            };
                }

                Log.Warning("Request failed: {StatusCode} - {ResponseContent}", response.StatusCode, responseContent);
                // Deserialize error response
                if (IsJson(responseContent))
                {
                    var errorObject = JsonSerializer.Deserialize<JsonObject>(responseContent) ?? new JsonObject();
                    return new JsonObject
            {
                { "isSuccess", false },
                { "message", errorObject.ContainsKey("message") ? errorObject["message"] : "Invalid User ID or Password." }
            };
                }
                else
                {
                    // If responseContent is not JSON, return it as an error message
                    return new JsonObject
            {
                { "isSuccess", false },
                { "message", responseContent } // Handle plain text error messages
            };
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception in GetUserByCred");
                return new JsonObject { { "isSuccess", false }, { "message", "An unexpected error occurred. Please try again later." } };
            }
        }

        private bool IsJson(string input)
        {
            input = input.Trim();
            return (input.StartsWith("{") && input.EndsWith("}")) || (input.StartsWith("[") && input.EndsWith("]"));
        }



    }
}
