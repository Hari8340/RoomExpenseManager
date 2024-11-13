using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using RoomExpenseManagerWebApp.Models;
using RoomExpenseManagerWebApp.Services.Interface.IExpense;
using RoomExpenseManagerWebApp.Services.Interface.ILogin;
using RoomExpenseManagerWebApp.Services.Interface.IUser;
using Serilog;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace RoomExpenseManagerWebApp.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogin _login;
        private readonly IUser _user;

        public LoginController(ILogin login, IUser user)
        {
            _login = login;
            _user = user;
        }
        [HttpGet]
        public IActionResult Login()
        {
            HttpContext.Session.Clear();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Models.Login login)
        {
            try
            {
                if (login == null || string.IsNullOrWhiteSpace(login.Id) || string.IsNullOrWhiteSpace(login.Password))
                {
                    return Json(new { isSuccess = false, message = "User ID and Password are required." });
                }

                var response = await _login.GetUserByCred(login);
                
                
                // Check if response indicates success
                if (response.ContainsKey("isSuccess") && response["isSuccess"].ToString() == "true")
                {

                    
                    // Deserialize the JSON string into the Login object
                    var loginResponse = JsonConvert.DeserializeObject<Login>(response["data"].ToString());
                    var userId = loginResponse.UserId;
                    var userResponse = await _user.GetUserByIdAsync(userId);
                    HttpContext.Session.SetInt32("UserId", userId);
                    HttpContext.Session.SetString("Image", userResponse.ImageBase64);
                    HttpContext.Session.SetString("Name", userResponse.Name);
                    return Json(new { isSuccess = true, message = "Login successful." });
                }
                else if (response.ContainsKey("message"))
                {
                    // If there's a message in the response, return it
                    return Json(new { isSuccess = false, message = response["message"].ToString() });
                }

                // If no specific message, return a generic invalid message
                return Json(new { isSuccess = false, message = "Invalid User ID or Password." });
            }
            catch (Exception ex)
            {
                // Log the exception for troubleshooting
                Log.Error(ex, "Error occurred during login.");
                return Json(new { isSuccess = false, message = "An unexpected error occurred. Please try again later." });
            }
        }



    }
}
