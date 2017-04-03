using System;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class OverurenBesparingController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public OverurenBesparingController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            OverurenBesparingViewModel model = MaakModel(analyse);

            return View(model);
        }

        public IActionResult Opslaan(Analyse analyse, OverurenBesparingViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                OverurenBesparing baat = new OverurenBesparing
                {
                    Type = model.Type,
                    Soort = model.Soort,
                    Bedrag = model.Bedrag
                };

                analyse.OverurenBesparing = baat;
                analyse.DatumLaatsteAanpassing = DateTime.Now;
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De baat is succesvol opgeslaan.";
            }

            return RedirectToAction("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse)
        {
            // Baat eruit halen
            analyse.OverurenBesparing = null;

            // Datum updaten
            analyse.DatumLaatsteAanpassing = DateTime.Now;

            // Opslaan
            try
            {
                _analyseRepository.Save();
                TempData["message"] = "De baat is succesvol verwijderd.";
            }
            catch
            {
                TempData["error"] = "Er ging iets mis tijdens het verwijderen, probeer het later opnieuw.";
            }

            OverurenBesparingViewModel model = MaakModel(analyse);

            return View("Index", model);
        }

        private OverurenBesparingViewModel MaakModel(Analyse analyse)
        {
            if (analyse.OverurenBesparing == null)
            {
                return new OverurenBesparingViewModel();
            }

            return new OverurenBesparingViewModel(analyse.OverurenBesparing);
        }
    }
}
