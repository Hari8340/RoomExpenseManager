using Microsoft.AspNetCore.Mvc;
using RoomExpenseManagerWebApp.Models;
using RoomExpenseManagerWebApp.Services.Interface.IExpense;
using RoomExpenseManagerWebApp.Services.Interface.IUser;
using Serilog;
using System.Net.Http;

namespace RoomExpenseManagerWebApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUser _user;
       
        public UserController(IUser user,IExpense expense)
        {
            _user = user;
           
                
        }
        public async Task<IActionResult> Index()
        {

            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                var users = await _user.GetAllUsersAsync();
                return View(users);
            }
            else
            {
                return RedirectToAction("Unauthorized", "Unauthorized");
            }
           
        }
        [HttpGet]
        public IActionResult CreateUser()
        
        {
            if (HttpContext.Session.GetInt32("UserId") != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Unauthorized", "Unauthorized");
            }
        }
        
        [HttpPost]
        // Ensures CSRF protection
        public async Task<IActionResult> CreateUser([FromForm] UserRequest userRequest)
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    await _user.CreateUserAsync(userRequest);
                    return Ok(new { message = "User created successfully." }); // 200 OK response
                }
                else
                {
                    return RedirectToAction("Unauthorized", "Unauthorized");
                }

            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "An error occurred while creating the user." }); // 500 Internal Server Error
            }
        }
        [HttpPut]
        // Ensures CSRF protection
        public async Task<IActionResult> UpdateUser(int userId,[FromForm] UserRequest userRequest)
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    await _user.UpdateUserAsync(userId, userRequest);
                    return Ok(new { message = "User updated successfully." }); // 200 OK response
                }
                else
                {
                    return RedirectToAction("Unauthorized", "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "An error occurred while creating the user." }); // 500 Internal Server Error
            }
        }

        [HttpDelete]

        // Ensures CSRF protection
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                if (HttpContext.Session.GetInt32("UserId") != null)
                {
                    await _user.DeleteUserAsync(id);
                    return Ok(new { message = "User created successfully." }); // 200 OK response
                }
                else
                {
                    return RedirectToAction("Unauthorized", "Unauthorized");
                }
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "An error occurred while creating the user." }); // 500 Internal Server Error
            }
        }




    }
}
