using Microsoft.AspNetCore.Mvc;

namespace VKR_server.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
