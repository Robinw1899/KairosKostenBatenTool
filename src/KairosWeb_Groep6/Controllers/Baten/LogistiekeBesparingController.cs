using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Authorization;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    public class LogistiekeBesparingController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public LogistiekeBesparingController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            LogistiekeBesparingViewModel model = new LogistiekeBesparingViewModel(analyse.LogistiekeBesparing);

            return View(model);
        }
        #endregion

        #region Opslaan
        public IActionResult Opslaan(Analyse analyse, LogistiekeBesparingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    LogistiekeBesparing baat = new LogistiekeBesparing
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        TransportKosten = model.TransportKosten,
                        LogistiekHandlingsKosten = model.LogistiekHandlingsKosten
                    };

                    analyse.LogistiekeBesparing = baat;
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();
                }

                TempData["message"] = Meldingen.OpslaanSuccesvolBaat;
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
                analyse.LogistiekeBesparing = null;

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
    }
}
