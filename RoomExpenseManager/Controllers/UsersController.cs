using Microsoft.AspNetCore.Mvc;
using RoomExpenseManager.Interfaces;
using RoomExpenseManager.Models;
using Serilog;
using static System.Net.Mime.MediaTypeNames;
namespace RoomExpenseManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            
            return Ok(users);
        }
        [HttpGet("aadhar/{id}")]
        public async Task<IActionResult> GetUserAadhar(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null || user.AadharPdfData == null)
            {
                return NotFound();
            }

            return File(user.AadharPdfData, "application/pdf");
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            return Ok(new UserResponse
            {
                UserId = user.UserId,
                ImageData=user.ImageData,
                Name = user.Name
            });
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<ActionResult<UserResponse>> CreateUser([FromForm] UserRequest userRequest)
        {
            Log.Information("entered into CreateUserMethod");
            try
            {
                var image = userRequest.Image;
                var aadharPdf = userRequest.AadharPdf;
                //Validate image type(only jpg and png allowed)
                //if (image == null || (userRequest.Image.ContentType != "image/jpeg" && image.ContentType != "image/png"))
                //{
                //    return BadRequest("Only JPG and PNG image formats are allowed.");
                //}

                // Read image as byte array
                using var imageMemoryStream = new MemoryStream();
                await image.CopyToAsync(imageMemoryStream);

                // Read Aadhaar PDF as byte array
                using var pdfMemoryStream = new MemoryStream();
                await aadharPdf.CopyToAsync(pdfMemoryStream);

                // Create user entity
                var user = new User
                {
                    
                    Name = userRequest.Name,
                    ImageData = imageMemoryStream.ToArray(), // Save image as binary data
                    AadharPdfData = pdfMemoryStream.ToArray(), // Save PDF as binary data
                    DOB = userRequest.DOB,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = null
                };

                await _userService.AddUserAsync(user);

                return CreatedAtAction(nameof(GetUser), new { id = user.UserId }, new UserResponse
                {
                    UserId = user.UserId,
                    Name = user.Name
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPut]
        [Route("UpdateUser")]
        public async Task<ActionResult> UpdateUser([FromQuery] int id, [FromForm] UserRequest userRequest)
        {
            try
            {
                // Fetch the user
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                // Update ImageData only if a new image is provided
                if (userRequest.Image != null)
                {
                    using var imageMemoryStream = new MemoryStream();
                    await userRequest.Image.CopyToAsync(imageMemoryStream);
                    user.ImageData = imageMemoryStream.ToArray(); // Update with new image
                }

                // Update AadharPdfData only if a new PDF is provided
                if (userRequest.AadharPdf != null)
                {
                    using var pdfMemoryStream = new MemoryStream();
                    await userRequest.AadharPdf.CopyToAsync(pdfMemoryStream);
                    user.AadharPdfData = pdfMemoryStream.ToArray(); // Update with new PDF
                }

                // Always update other details
                user.Name = userRequest.Name;
                user.DOB = userRequest.DOB;
                user.UpdatedDate = DateTime.UtcNow;

                // Save changes
                await _userService.UpdateUserAsync(user);

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { message = "An error occurred while updating the user." });
            }
        }


        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<ActionResult<bool>> DeleteUser([FromQuery] int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null) return NotFound();

                bool res = await _userService.DeleteUserAsync(id);

                // Optionally, you can check the result of the deletion
                if (!res)
                {
                    return StatusCode(500, new { message = "Failed to delete user." }); // Optional: if deletion failed
                }

                return NoContent(); // Return 204 No Content if successful
            }
            catch (Exception ex)
            {
                // Log the exception (optional)
                Console.WriteLine(ex.Message); // or use a logging framework
                return StatusCode(500, new { message = "An error occurred while deleting the user." }); // 500 Internal Server Error
            }
        }

    }
}
