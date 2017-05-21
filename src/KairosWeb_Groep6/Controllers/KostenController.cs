using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;
using Microsoft.AspNetCore.Authorization;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    public class KostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public KostenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        [ServiceFilter(typeof(AnalyseFilter))]
        public IActionResult Index(Analyse analyse)
        {
            analyse = _analyseRepository.GetByIdAll(analyse.AnalyseId);
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

        public IActionResult PersoneelsKosten()
        {
            return RedirectToAction("Index", "PersoneelsKosten");
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
