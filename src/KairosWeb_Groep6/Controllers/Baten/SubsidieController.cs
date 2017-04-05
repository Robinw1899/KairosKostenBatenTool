using System;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class SubsidieController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public SubsidieController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            SubsidieViewModel model = MaakModel(analyse);

            return View(model);
        }

        [HttpPost]
        public IActionResult Opslaan(Analyse analyse, SubsidieViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                Subsidie baat = new Subsidie
                {
                    Type = model.Type,
                    Soort = model.Soort,
                    Bedrag = model.Bedrag
                };

                analyse.Subsidie = baat;
                analyse.DatumLaatsteAanpassing = DateTime.Now;
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De baat is succesvol opgeslaan.";
            }

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse)
        {
            // Baat eruit halen
            analyse.Subsidie = null;

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

            SubsidieViewModel model = MaakModel(analyse);

            return View("Index", model);
        }

        private SubsidieViewModel MaakModel(Analyse analyse)
        {
            if (analyse.Subsidie == null)
            {
                return new SubsidieViewModel();
            }

            return new SubsidieViewModel(analyse.Subsidie);
        }
    }
}
