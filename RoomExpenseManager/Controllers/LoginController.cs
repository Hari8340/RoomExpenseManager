using Microsoft.AspNetCore.Mvc;
using RoomExpenseManager.Implementation;
using RoomExpenseManager.Interfaces;
using RoomExpenseManager.Models;
using Serilog;

namespace RoomExpenseManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly ILogin _login;

        public LoginController(ILogin login)
        {
            _login = login;
        }

        [HttpGet]
        [Route("GetLogins")]
        public async Task<ActionResult<IEnumerable<Login>>> GetLogins()
        {
            try
            {
                Log.Information("Entering GetAllLogins method");
                var users = await _login.GetAllLogins();
                
                Log.Information("Exiting GetAllLogins method with response: {Response}");
                return Ok(users);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in GetAllLogins method");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpGet]
        [Route("GetUserLoginById")]
        public async Task<ActionResult<Login>> GetLoginByUserId(string userId, string password)
        {
            try
            {
                Log.Information("Entering GetLoginByUserId method with userId: {UserId}", userId);

                Login user = null;

                // Wrap repository call in try-catch if the service might throw an exception
                try
                {
                    user = await _login.GetLoginById(userId);
                }
                catch (Exception ex)
                {
                    Log.Warning(ex, "User not found for userId: {UserId}", userId);
                    return NotFound("User not found. Please enter a correct username.");
                }

                if (user == null)
                {
                    Log.Warning("User not found for userId: {UserId}", userId);
                    return NotFound("User not found. Please enter a correct username.");
                }

                // Check if the password is correct
                if (user.Password != password)
                {
                    Log.Warning("Invalid password for userId: {UserId}", userId);
                    return BadRequest("Invalid password.");
                }

                Log.Information("Exiting GetLoginByUserId method with response: {Response}", user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error occurred in GetLoginByUserId method");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
