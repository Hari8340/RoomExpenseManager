using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace RoomExpenseManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("GetEmployee")]
        public ContentResult Employee()
        {
            return Content("i am from my controller");
        }
    }
}
