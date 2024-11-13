using RoomExpenseManagerWebApp.Constants;
using RoomExpenseManagerWebApp.Models;
using RoomExpenseManagerWebApp.Services.Interface.IUser;
using Serilog;
using System.IO;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace RoomExpenseManagerWebApp.Services.Implementation.User
{
    public class User : IUser
    {
        private readonly HttpClient _httpClient;

        public User(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserRequest> CreateUserAsync(UserRequest user)
        {

            using (var form = new MultipartFormDataContent())
            {
                form.Add(new StringContent(user.UserId.ToString()), "UserId");
                form.Add(new StringContent(user.Name), "Name");
                form.Add(new StringContent(user.DOB.ToString()), "DOB");
                form.Add(new StringContent(DateTime.UtcNow.ToString("o")), "CreatedDate"); // ISO 8601 format
                form.Add(new StringContent(DateTime.UtcNow.ToString("o")), "UpdatedDate");
                if (user.AadharPdf != null)
                {
                    var stream = user.AadharPdf.OpenReadStream();
                    form.Add(new StreamContent(stream), "AadharPdf", user.AadharPdf.FileName);
                }
                if (user.Image != null)
                {
                    var stream = user.Image.OpenReadStream();
                    form.Add(new StreamContent(stream), "Image", user.Image.FileName);
                }
                var response = await _httpClient.PostAsync(ApiRoutes.Users.Create, form);

                if (response.IsSuccessStatusCode)
                {
                    // Optionally deserialize the response if needed
                    return await response.Content.ReadFromJsonAsync<UserRequest>();
                }

                // Handle errors accordingly (throw an exception, return null, etc.)
                response.EnsureSuccessStatusCode(); // Throws if the status code does not indicate success


            }


            return null; // or handle according to your application's logic
        }
        public async Task<UserRequest> UpdateUserAsync(int userId, UserRequest user)
        {

            using (var form = new MultipartFormDataContent())
            {
                form.Add(new StringContent(user.UserId.ToString()), "UserId");
                form.Add(new StringContent(user.Name), "Name");
                form.Add(new StringContent(user.DOB.ToString()), "DOB");
                form.Add(new StringContent(DateTime.UtcNow.ToString("o")), "CreatedDate"); // ISO 8601 format
                form.Add(new StringContent(DateTime.UtcNow.ToString("o")), "UpdatedDate");
                if (user.AadharPdf != null)
                {
                    var stream = user.AadharPdf.OpenReadStream();
                    form.Add(new StreamContent(stream), "AadharPdf", user.AadharPdf.FileName);
                }

                if (user.Image != null)
                {
                    var stream = user.Image.OpenReadStream();
                    form.Add(new StreamContent(stream), "Image", user.Image.FileName);
                }
                var requestUri = $"http://localhost/api/Users/UpdateUser?id={userId}";
                var response = await _httpClient.PutAsync(requestUri, form);

                if (response.IsSuccessStatusCode)
                {
                    // Optionally deserialize the response if needed
                    return await response.Content.ReadFromJsonAsync<UserRequest>();
                }

                // Handle errors accordingly (throw an exception, return null, etc.)
                response.EnsureSuccessStatusCode(); // Throws if the status code does not indicate success


            }


            return null; // or handle according to your application's logic
        }


        public async Task<List<UserResponse>> GetAllUsersAsync()
        {
            Log.Information("Fetching all users.");
            var response = await _httpClient.GetAsync(ApiRoutes.Users.GetAll);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<UserResponse>>();
            }

            Log.Error("Error fetching users: {StatusCode}", response.StatusCode);
            return new List<UserResponse>();
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                //var requestUri = $"https://localhost:44374/api/Users/DeleteUser?id={id}";
                var response = await _httpClient.DeleteAsync(ApiRoutes.Users.Delete.Replace("{id}", id.ToString()));

                if (response.IsSuccessStatusCode)
                {
                    return true; // Successfully deleted
                }

                // Handle non-success status codes here if needed
                return false;
            }
            catch (Exception ex)
            {
                // Log the exception (if needed)
                Console.WriteLine(ex.Message);
                return false; // Indicate failure
            }

        }

        public async Task<UserResponse> GetUserByIdAsync(int userId)
        {
            Log.Information("Fetching all users.");
            var response = await _httpClient.GetAsync(ApiRoutes.Users.GetById.Replace("{id}", userId.ToString()));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserResponse>();
            }

            Log.Error("Error fetching users: {StatusCode}", response.StatusCode);
            return new UserResponse();
        }
    }
}
