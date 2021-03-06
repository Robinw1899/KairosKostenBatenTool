﻿using KairosWeb_Groep6.Filters;
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
            analyse = _analyseRepository.GetById(analyse.AnalyseId, Soort.LogistiekeBesparing);
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
                    analyse = _analyseRepository.GetById(analyse.AnalyseId, Soort.LogistiekeBesparing);
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
                if (e is ArgumentException || e is FormatException)
                {
                    TempData["error"] = e.Message;
                }
                else
                {
                    _exceptionLogRepository.Add(new ExceptionLog(e, "LogistiekeBesparing", "Opslaan -- GET --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
