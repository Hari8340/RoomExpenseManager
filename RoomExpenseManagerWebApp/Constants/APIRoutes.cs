using static System.Net.WebRequestMethods;

namespace RoomExpenseManagerWebApp.Constants
{
    public static class ApiRoutes
    {
        //private const string BaseUrl = "http://localhost/api";
        private const string BaseUrl = "https://localhost:44374/api";

        public static class Users
        {
            public const string GetAll = $"{BaseUrl}/Users";
            public const string GetById = $"{BaseUrl}/Users/{{id}}";  // Use with string interpolation
            public const string Create = $"{BaseUrl}/Users/CreateUser";
            public const string Update = $"{BaseUrl}/Users/UpdateUser/{{id}}";  // PUT method with ID
            public const string Delete = $"{BaseUrl}/Users/DeleteUser?id={{id}}";// DELETE method with ID
        }
        public static class Expenses
        {
            //private const string BaseUrl = "http://localhost/api"; // Ensure this matches your API base URL

            public const string GetAll = $"{BaseUrl}/Expenses/GetExpenses"; // GET method to retrieve all expenses
            public const string GetById = $"{BaseUrl}/Expenses/GetExpenseById?id={{id}}"; // GET method to retrieve a specific expense by ID
            public const string Create = $"{BaseUrl}/Expenses/CreateExpense"; // POST method to create a new expense
            public const string Update = $"{BaseUrl}/Expenses/UpdateExpense?id={{id}}"; // PUT method to update an existing expense by ID
            public const string Delete = $"{BaseUrl}/Expenses/DeleteExpense?id={{id}}"; // DELETE method to remove an expense by ID
        }
        public static class Login
        {
            //private const string BaseUrl = "https://localhost/api"; // Ensure this matches your API base URL
            public const string GetById = $"{BaseUrl}/Login/GetUserLoginById"; // GET method to retrieve a specific expense by ID
           
        }

    }
}
