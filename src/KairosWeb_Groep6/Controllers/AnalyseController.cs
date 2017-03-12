using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    public class AnalyseController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Baten");
        }

        public IActionResult NieuweAnalyse()
        {
            return View();
        }
    }
}
