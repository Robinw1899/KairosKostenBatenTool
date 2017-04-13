using System;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using Microsoft.AspNetCore.Authorization;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [Authorize]
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

                    TempData["message"] = Meldingen.OpslaanSuccesvolBaat;
                }
            }
            catch
            {
                TempData["error"] = Meldingen.OpslaanFoutmeldingBaat;
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
                analyse.ExtraOmzet = null;

                // Datum updaten
                analyse.DatumLaatsteAanpassing = DateTime.Now;

                // Opslaan
                _analyseRepository.Save();
                TempData["message"] = Meldingen.VerwijderSuccesvolBaat;
            }
            catch
            {
                TempData["error"] = Meldingen.VerwijderFoutmeldingBaat;
            }

            return RedirectToAction("Index");
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
