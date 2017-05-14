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
    [AutoValidateAntiforgeryToken]
    public class ExtraOmzetController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;

        public ExtraOmzetController(IAnalyseRepository analyseRepository,
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
                    DecimalConverter dc = new DecimalConverter();
                    ExtraOmzet baat = new ExtraOmzet
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        JaarbedragOmzetverlies =dc.ConvertToDecimal( model.JaarbedragOmzetverlies),
                        Besparing = dc.ConvertToDecimal(model.Besparing)
                    };
                    
                    analyse.ExtraOmzet = baat;
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.OpslaanSuccesvolBaat;
                }
            }
            catch (Exception e)
            {
                if (e is ArgumentException || e is FormatException)
                {
                    TempData["error"] = e.Message;
                }
                else
                {
                    _exceptionLogRepository.Add(new ExceptionLog(e, "ExtraOmzet", "Opslaan -- GET --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
                    return RedirectToAction("Index");
                }
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
