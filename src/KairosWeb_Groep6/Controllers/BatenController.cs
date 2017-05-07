using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    public class BatenController : Controller
    {
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult Index(Analyse analyse)
        {
            BatenIndexViewModel model = new BatenIndexViewModel(analyse);

            return View(model);
        }

        public IActionResult MedewerkerZelfdeNiveau()
        {
            return RedirectToAction("Index", "MedewerkersZelfdeNiveau");
        }

        public IActionResult MedewerkerHogerNiveau()
        {
            return RedirectToAction("Index", "MedewerkersHogerNiveau");
        }

        public IActionResult UitzendKrachtBesparingen()
        {
            return RedirectToAction("Index", "UitzendKrachtBesparingen");
        }

        public IActionResult ExtraOmzet()
        {
            return RedirectToAction("Index", "ExtraOmzet");
        }

        public IActionResult ExtraProductiviteit()
        {
            return RedirectToAction("Index", "ExtraProductiviteit");
        }

        public IActionResult OverurenBesparing()
        {
            return RedirectToAction("Index", "OverurenBesparing");
        }

        public IActionResult ExterneInkopen()
        {
            return RedirectToAction("Index", "ExterneInkopen");
        }

        public IActionResult Subsidie()
        {
            return RedirectToAction("Index", "Subsidie");
        }

        public IActionResult LogistiekeBesparing()
        {
            return RedirectToAction("Index", "LogistiekeBesparing");
        }

        public IActionResult ExtraBesparingen()
        {
            return RedirectToAction("Index", "ExtraBesparingen");
        }
    }
}
