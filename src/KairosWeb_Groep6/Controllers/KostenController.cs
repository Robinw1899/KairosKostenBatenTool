using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;

namespace KairosWeb_Groep6.Controllers
{
    public class KostenController : Controller
    {
        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult Index(Analyse analyse)
        {
            KostenIndexViewModel model = new KostenIndexViewModel(analyse);

            return View(model);
        }

        public IActionResult BegeleidingsKosten()
        {
            return RedirectToAction("Index", "BegeleidingsKosten");
        }

        public IActionResult EnclaveKosten()
        {
            return RedirectToAction("Index", "EnclaveKosten");
        }

        public IActionResult ExtraKosten()
        {
            return RedirectToAction("Index", "ExtraKosten");
        }

        public IActionResult GereedschapsKosten()
        {
            return RedirectToAction("Index", "GereedschapsKosten");
        }

        public IActionResult InfrastructuurKosten()
        {
            return RedirectToAction("Index", "InfrastructuurKosten");
        }

        public IActionResult Loonkosten()
        {
            return RedirectToAction("Index", "Loonkosten");
        }

        public IActionResult OpleidingsKosten()
        {
            return RedirectToAction("Index", "OpleidingsKosten");
        }

        public IActionResult VoorbereidingsKosten()
        {
            return RedirectToAction("Index", "VoorbereidingsKosten");
        }
    }
}
