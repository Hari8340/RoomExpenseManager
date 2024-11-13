using Microsoft.AspNetCore.Mvc;

namespace RoomExpenseManagerWebApp.Controllers
{
    public class UnauthorizedController : Controller
    {
        public IActionResult Unauthorized()
        {
            return View();
        }
    }
}
