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
            return RedirectToAction("Index", "MedewerkerZelfdeNiveau");
        }

        public IActionResult MedewerkerHogerNiveau()
        {
            return RedirectToAction("Index", "MedewerkerHogerNiveau");
        }

        public IActionResult UitzendKrachtBesparingen()
        {
            return RedirectToAction("Index", "UitzendKrachtBesparing");
        }

        public IActionResult ExtraOmzet()
        {
            return RedirectToAction("Index", "ExtraOmzet");
        }

        public IActionResult ExtraProductiviteit()
        {
            return RedirectToAction("Index", "ExtraProductiviteit");
        }

        public IActionResult OverurenBesparingen()
        {
            return RedirectToAction("Index", "OverurenBesparing");
        }

        public IActionResult ExterneInkopen()
        {
            return RedirectToAction("Index", "ExterneInkoop");
        }

        public IActionResult Subsidies()
        {
            return RedirectToAction("Index", "Subsidie");
        }

        public IActionResult ExtraBesparingen()
        {
            return RedirectToAction("Index", "ExtraBesparing");
        }
    }
}
