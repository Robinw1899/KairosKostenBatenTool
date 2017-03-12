using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
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

        public IActionResult NieuweWerkgever()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NieuweWerkgever(int id)
        {
            return View();
        }
    }
}
