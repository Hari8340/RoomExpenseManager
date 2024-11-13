using Microsoft.AspNetCore.Mvc;

namespace RoomExpenseManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : Controller
    {
        [HttpGet]
        [Route("GetEmployee")]
        public ContentResult Employee()
        {
            return Content("i am from my controller");
        }
    }
}
