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
    public class ExtraProductiviteitController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;

        public ExtraProductiviteitController(IAnalyseRepository analyseRepository,
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
                    DecimalConverter dc = new DecimalConverter();
                    ExtraProductiviteit baat = new ExtraProductiviteit
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Bedrag = dc.ConvertToDecimal(model.Bedrag)
                    };

                    analyse.ExtraProductiviteit = baat;
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
                    _exceptionLogRepository.Add(new ExceptionLog(e, "ExtraProductiviteit", "Opslaan -- GET --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
                    return RedirectToAction("Index");
                }
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
