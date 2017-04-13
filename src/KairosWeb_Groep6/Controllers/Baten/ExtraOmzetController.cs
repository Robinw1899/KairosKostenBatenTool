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

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            ExtraOmzetViewModel model = MaakModel(analyse);

            return View(model);
        }
        #endregion

        #region Opslaan
        public IActionResult Opslaan(Analyse analyse, ExtraOmzetViewModel model)
        {
            try
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
            }
            catch
            {
                TempData["error"] = "Er ging iets mis, probeer later opnieuw";
            }
            
            return PartialView("_Formulier", model);
        }
        #endregion

        #region Verwijder
        public IActionResult Verwijder(Analyse analyse)
        {
            try
            {
                // Baat eruit halen
                analyse.ExtraOmzet = null;

                // Datum updaten
                analyse.DatumLaatsteAanpassing = DateTime.Now;

                // Opslaan
                _analyseRepository.Save();
                TempData["message"] = "De baat is succesvol verwijderd.";
            }
            catch
            {
                TempData["error"] = "Er ging iets mis, probeer later opnieuw";
                return RedirectToAction("Index");
            }

            return PartialView("_Formulier", MaakModel(analyse));
        }
        #endregion

        #region Helpers
        private ExtraOmzetViewModel MaakModel(Analyse analyse)
        {
            if (analyse.ExtraOmzet == null)
            {
                return new ExtraOmzetViewModel();
            }

            return new ExtraOmzetViewModel(analyse.ExtraOmzet);
        }
        #endregion
    }
}
