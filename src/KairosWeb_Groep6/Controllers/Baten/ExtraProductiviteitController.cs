using System;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class ExtraProductiviteitController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public ExtraProductiviteitController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            var model = MaakModel(analyse);

            return View(model);
        }
        #endregion

        #region Opslaan
        public IActionResult Opslaan(Analyse analyse, ExtraProductiviteitViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ExtraProductiviteit baat = new ExtraProductiviteit
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Bedrag = model.Bedrag
                    };

                    analyse.ExtraProductiviteit = baat;
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = "De baat is succesvol opgeslaan.";
                }
            }
            catch
            {
                TempData["error"] = "Er ging iets mis tijdens het opslaan, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Verwijder
        public IActionResult Verwijder(Analyse analyse)
        {
            try
            {
                // Baat eruit halen
                analyse.ExtraProductiviteit = null;

                // Datum updaten
                analyse.DatumLaatsteAanpassing = DateTime.Now;

                // Opslaan
                _analyseRepository.Save();
                TempData["message"] = "De baat is succesvol verwijderd.";
            }
            catch
            {
                TempData["error"] = "Er ging iets mis tijdens het verwijderen, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Helpers
        private ExtraProductiviteitViewModel MaakModel(Analyse analyse)
        {
            if (analyse.ExtraProductiviteit == null)
            {
                return new ExtraProductiviteitViewModel();
            }
           
            return new ExtraProductiviteitViewModel(analyse.ExtraProductiviteit);
        }
        #endregion
    }
}
