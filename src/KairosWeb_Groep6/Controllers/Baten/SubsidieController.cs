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
    public class SubsidieController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;

        public SubsidieController(IAnalyseRepository analyseRepository,
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

            SubsidieViewModel model = MaakModel(analyse);

            return View(model);
        }
        #endregion

        #region Opslaan
        [HttpPost]
        public IActionResult Opslaan(Analyse analyse, SubsidieViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // de baat bestaat reeds:
                    DecimalConverter dc = new DecimalConverter();
                    Subsidie baat = new Subsidie
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Bedrag = dc.ConvertToDecimal(model.Bedrag)
                    };

                    analyse.Subsidie = baat;
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
                    _exceptionLogRepository.Add(new ExceptionLog(e, "Subsidie", "Opslaan -- POST --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
                    return RedirectToAction("Index");
                }
            }


            return RedirectToAction("Index");
        }
        #endregion      

        #region Helpers
        private SubsidieViewModel MaakModel(Analyse analyse)
        {
            if (analyse.Subsidie == null)
            {
                return new SubsidieViewModel();
            }

            return new SubsidieViewModel(analyse.Subsidie);
        }
        #endregion
    }
}
