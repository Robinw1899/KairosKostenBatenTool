using System;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class ExtraOmzetController : Controller
    {

        private readonly IAnalyseRepository _analyseRepository;

        public ExtraOmzetController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            ExtraOmzetViewModel model = MaakModel(analyse);

            return View(model);
        }

        public IActionResult Opslaan(Analyse analyse, ExtraOmzetViewModel model)
        {
            if (ModelState.IsValid)
            {
                ExtraOmzet baat = new ExtraOmzet
                {
                    Type = model.Type,
                    Soort = model.Soort,
                    JaarbedragOmzetverlies = model.JaarbedragOmzetverlies,
                    Besparing = model.Besparing
                };

                analyse.ExtraOmzet = baat;
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
            analyse.ExtraOmzet = null;

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

            ExtraOmzetViewModel model = MaakModel(analyse);

            return View("Index", model);
        }

        private ExtraOmzetViewModel MaakModel(Analyse analyse)
        {
            if (analyse.ExtraOmzet == null)
            {
                return new ExtraOmzetViewModel();
            }

            return new ExtraOmzetViewModel(analyse.ExtraOmzet);
        }
    }
}
