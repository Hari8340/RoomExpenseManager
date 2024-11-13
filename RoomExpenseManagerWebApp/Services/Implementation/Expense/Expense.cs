using RoomExpenseManagerWebApp.Constants;
using RoomExpenseManagerWebApp.Models;
using RoomExpenseManagerWebApp.Services.Interface.IExpense;
using Serilog;

namespace RoomExpenseManagerWebApp.Services.Implementation.Expense
{
    public class Expense : IExpense
    {
        private readonly HttpClient _httpClient;

        public Expense(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Models.Expense> CreateExpenseAsync(Models.Expense expense)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ApiRoutes.Expenses.Create, expense);

                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response if needed
                    return await response.Content.ReadFromJsonAsync<Models.Expense>();
                }
                else
                {
                    // Handle unsuccessful response
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Error creating expense: {response.StatusCode} - {errorContent}");
                }
            }
            catch (HttpRequestException ex)
            {
                // Handle network-related errors
                throw new Exception("Network error while creating expense.", ex);
            }
            catch (Exception ex)
            {
                // Handle other potential exceptions
                throw new Exception("An error occurred while creating expense.", ex);
            }
        }


        public async Task<bool> DeleteExpenseAsync(int id)
        {
            Log.Information("Fetching expense with ID: {Id}", id);
            //var requestUri = $"http://localhost/api/Expenses/DeleteExpense?id={id}";
            var response = await _httpClient.DeleteAsync(ApiRoutes.Expenses.Delete.Replace("{id}",id.ToString()));
            //var response = await _httpClient.DeleteAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            Log.Error("Error fetching expense with ID: {Id}. Status Code: {StatusCode}", id, response.StatusCode);
            return false; // Or throw an exception based on your error handling policy.
        }

        public async Task<IEnumerable<ExpenseResponse>> GetAllExpenseAsync()
        {
            Log.Information("Fetching all expenses.");
            var response = await _httpClient.GetAsync(ApiRoutes.Expenses.GetAll);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize directly to List<ExpenseResponse>
                return await response.Content.ReadFromJsonAsync<IEnumerable<ExpenseResponse>>();
            }

            Log.Error("Error fetching expenses: {StatusCode}", response.StatusCode);
            return new List<ExpenseResponse>(); // Return an empty list on error
        }


        public async Task<ExpenseResponse> GetExpenseByIdAsync(int id)
        {
            Log.Information("Fetching expense with ID: {Id}", id);
            var response = await _httpClient.GetAsync(ApiRoutes.Expenses.GetById.Replace("{id}", id.ToString()));

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<ExpenseResponse>();
            }

            Log.Error("Error fetching expense with ID: {Id}. Status Code: {StatusCode}", id, response.StatusCode);
            return null; // Or throw an exception based on your error handling policy.
        }

        public async Task<Models.Expense> UpdateExpenseAsync(int expenseId, Models.Expense expense)
        {
            Log.Information("Fetching expense with ID: {Id}", expenseId);
            var response = await _httpClient.PutAsJsonAsync(ApiRoutes.Expenses.Update.Replace("{id}", expenseId.ToString()), expense);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<Models.Expense>();
            }

            Log.Error("Error fetching expense with ID: {Id}. Status Code: {StatusCode}", expenseId, response.StatusCode);
            return null; // Or throw an exception based on your error handling policy.
        }

       
    }
}
