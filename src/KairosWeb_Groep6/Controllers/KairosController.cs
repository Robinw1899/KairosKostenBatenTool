using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    public class KairosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NieuweAnalyse()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Opmerking()
        {
            //throw new System.NotImplementedException();
            return View();
        }
    }
}
