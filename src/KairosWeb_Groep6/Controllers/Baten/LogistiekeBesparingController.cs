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
    [AutoValidateAntiforgeryToken]
    public class LogistiekeBesparingController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;

        public LogistiekeBesparingController(IAnalyseRepository analyseRepository,
            IExceptionLogRepository exceptionLogRepository)
        {
            _analyseRepository = analyseRepository;
            _exceptionLogRepository = exceptionLogRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            if (analyse.Klaar)
            {
                TempData["error"] = Meldingen.AnalyseKlaar;
                return RedirectToAction("Index", "Resultaat");
            }

            analyse.UpdateTotalen(_analyseRepository);

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
                    DecimalConverter dc = new DecimalConverter();
                    LogistiekeBesparing baat = new LogistiekeBesparing
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        TransportKosten = dc.ConvertToDecimal(model.TransportKosten),
                        LogistiekHandlingsKosten =dc.ConvertToDecimal(model.LogistiekHandlingsKosten)
                    };

                    analyse.LogistiekeBesparing = baat;
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();
                }

                TempData["message"] = Meldingen.OpslaanSuccesvolBaat;
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "LogistiekeBesparing", "Opslaan"));
                _exceptionLogRepository.Save();
                TempData["error"] = Meldingen.OpslaanFoutmeldingBaat;
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
