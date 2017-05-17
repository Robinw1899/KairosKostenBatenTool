using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using Microsoft.AspNetCore.Authorization;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    [AutoValidateAntiforgeryToken]
    public class ExterneInkopenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;

        public ExterneInkopenController(IAnalyseRepository analyseRepository,
            IExceptionLogRepository exceptionLogRepository)
        {
            _analyseRepository = analyseRepository;
            _exceptionLogRepository = exceptionLogRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId, Soort.ExterneInkoop);
            analyse.UpdateTotalen(_analyseRepository);

            IEnumerable<ExterneInkoopViewModel> viewModels = MaakModel(analyse);

            PlaatsTotaalInViewData(analyse);

            return View(viewModels);
        }
        #endregion

        #region VoegToe
        public IActionResult VoegToe()
        {
            ExterneInkoopViewModel model = new ExterneInkoopViewModel();
            return PartialView("_Formulier", model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, ExterneInkoopViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    analyse = _analyseRepository.GetById(analyse.AnalyseId, Soort.ExterneInkoop);
                    DecimalConverter dc = new DecimalConverter();
                    ExterneInkoop baat = new ExterneInkoop
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Beschrijving = model.Beschrijving,
                        Bedrag = dc.ConvertToDecimal(model.Bedrag)
                    };

                    analyse.ExterneInkopen.Add(baat);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.VoegToeSuccesvolBaat;
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
                    _exceptionLogRepository.Add(new ExceptionLog(e, "ExterneInkopen", "VoegToe -- POST --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Bewerk
        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            try
            {
                analyse = _analyseRepository.GetById(analyse.AnalyseId, Soort.ExterneInkoop);

                ExterneInkoop baat = KostOfBaatExtensions.GetBy(analyse.ExterneInkopen, id);

                ExterneInkoopViewModel model = new ExterneInkoopViewModel();

                DecimalConverter dc = new DecimalConverter();

                if (baat != null)
                {
                    // parameters voor formulier instellen
                    model.Id = id;
                    model.Type = baat.Type;
                    model.Soort = baat.Soort;
                    model.Beschrijving = baat.Beschrijving;
                    model.Bedrag = dc.ConvertToString(baat.Bedrag);

                    return PartialView("_Formulier", model);
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
                    _exceptionLogRepository.Add(new ExceptionLog(e, "ExterneInkopen", "Bewerk -- GET --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, ExterneInkoopViewModel model)
        {// id is het id van de baat die moet bewerkt worden
            try
            {
                analyse = _analyseRepository.GetById(analyse.AnalyseId, Soort.ExterneInkoop);

                ExterneInkoop baat = KostOfBaatExtensions.GetBy(analyse.ExterneInkopen, model.Id);
                DecimalConverter dc = new DecimalConverter();
                if (ModelState.IsValid && baat != null)
                {
                    baat.Id = model.Id;
                    baat.Type = model.Type;
                    baat.Soort = model.Soort;
                    baat.Beschrijving = model.Beschrijving;
                    baat.Bedrag = dc.ConvertToDecimal(model.Bedrag);

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
                    _exceptionLogRepository.Add(new ExceptionLog(e, "ExterneInkopen", "Bewerk -- POST --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Verwijder
        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            try
            {
                analyse = _analyseRepository.GetById(analyse.AnalyseId, Soort.ExterneInkoop);
                ExterneInkoop baat = KostOfBaatExtensions.GetBy(analyse.ExterneInkopen, id);

                if (baat != null)
                {
                    analyse.ExterneInkopen.Remove(baat);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();
                }
            }
            catch(Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "ExterneInkopen", "Verwijder"));
                _exceptionLogRepository.Save();
                TempData["error"] = Meldingen.VerwijderFoutmeldingBaat;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Helpers
        private IEnumerable<ExterneInkoopViewModel> MaakModel(Analyse analyse)
        {
            return analyse
                .ExterneInkopen
                .Select(m => new ExterneInkoopViewModel(m))
                .ToList();
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.ExterneInkopen.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            decimal totaal = analyse.ExterneInkopen
                                    .Sum(t => t.Bedrag);

            ViewData["totaal"] = totaal.ToString("C", new CultureInfo("nl-BE"));
        }
        #endregion
    }
}
