using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using Microsoft.AspNetCore.Mvc;
using System;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class LogistiekeBesparingController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public LogistiekeBesparingController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            LogistiekeBesparingViewModel model = new LogistiekeBesparingViewModel(analyse.LogistiekeBesparing);

            return View(model);
        }

        public IActionResult Opslaan(Analyse analyse, LogistiekeBesparingViewModel model)
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

                try
                {
                    _analyseRepository.Save();
                    TempData["message"] = "De baat is succesvol opgeslaan.";
                }
                catch
                {
                    TempData["error"] = "Er ging iets mis tijdens het verwijderen, probeer het later opnieuw.";
                }

                model = new LogistiekeBesparingViewModel(analyse.LogistiekeBesparing);
            }

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse)
        {
            // Baat eruit halen
            analyse.LogistiekeBesparing = null;

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

            LogistiekeBesparingViewModel model = new LogistiekeBesparingViewModel(analyse.LogistiekeBesparing);

            return View("Index", model);
        }
    }
}
