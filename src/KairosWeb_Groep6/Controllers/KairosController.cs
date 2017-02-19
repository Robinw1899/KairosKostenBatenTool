using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    public class KairosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
