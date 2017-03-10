using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    public class BatenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MedewerkerZelfdeNiveau()
        {
            return RedirectToAction("Index", nameof(MedewerkerZelfdeNiveau));
        }

        public IActionResult MedewerkerHogerNiveau()
        {
            return RedirectToAction("Index", nameof(MedewerkerHogerNiveau));
        }

        public IActionResult UitzendKrachtBesparing()
        {
            return RedirectToAction("Index", nameof(UitzendKrachtBesparing));
        }
    }
}
