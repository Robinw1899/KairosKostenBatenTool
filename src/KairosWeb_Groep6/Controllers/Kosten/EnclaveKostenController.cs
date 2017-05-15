using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers.Kosten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    [AutoValidateAntiforgeryToken]
    public class EnclaveKostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;

        public EnclaveKostenController(IAnalyseRepository analyseRepository,
            IExceptionLogRepository exceptionLogRepository)
        {
            _analyseRepository = analyseRepository;
            _exceptionLogRepository = exceptionLogRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            analyse.UpdateTotalen(_analyseRepository);

            IEnumerable<EnclaveKostViewModel> viewModels = MaakModel(analyse);

            PlaatsTotaalInViewData(analyse);

            return View(viewModels);
        }
        #endregion

        #region VoegToe
        public IActionResult VoegToe()
        {
            EnclaveKostViewModel model = new EnclaveKostViewModel();
            return PartialView("_Formulier", model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, EnclaveKostViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DecimalConverter dc = new DecimalConverter();
                    EnclaveKost kost = new EnclaveKost
                    {
                        Soort = model.Soort,
                        Type = model.Type,
                        Beschrijving = model.Beschrijving,
                        Bedrag = dc.ConvertToDecimal(model.Bedrag)
                    };

                    analyse.EnclaveKosten.Add(kost);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.VoegToeSuccesvolKost;
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
                    _exceptionLogRepository.Add(new ExceptionLog(e, "EnclaveKost", "VoegToe -- POST --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
                    return RedirectToAction("Index");
                }
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index");
        }
        #endregion

        #region Bewerk
        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            try
            {
                EnclaveKost kost = KostOfBaatExtensions.GetBy(analyse.EnclaveKosten, id);
                EnclaveKostViewModel model = new EnclaveKostViewModel();
                DecimalConverter dc = new DecimalConverter();
                if (kost != null)
                {
                    // parameters voor formulier instellen
                    model.Id = id;
                    model.Type = kost.Type;
                    model.Beschrijving = kost.Beschrijving;
                    model.Soort = kost.Soort;
                    model.Bedrag = dc.ConvertToString(kost.Bedrag);

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
                    _exceptionLogRepository.Add(new ExceptionLog(e, "BegeleidingsKost", "Bewerk -- GET --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, EnclaveKostViewModel model)
        {
            try
            {
                EnclaveKost kost = KostOfBaatExtensions.GetBy(analyse.EnclaveKosten, model.Id);
                DecimalConverter dc = new DecimalConverter();
                if (ModelState.IsValid && kost != null)
                {
                    kost.Id = model.Id;
                    kost.Type = model.Type;
                    kost.Beschrijving = model.Beschrijving;
                    kost.Soort = model.Soort;
                    kost.Bedrag = dc.ConvertToDecimal(model.Bedrag);

                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.OpslaanSuccesvolKost;
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
                    _exceptionLogRepository.Add(new ExceptionLog(e, "BegeleidingsKost", "Bewerk -- POST --"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
                    return RedirectToAction("Index");
                }
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index");
        }
        #endregion

        #region Verwijder
        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de kost die moet verwijderd worden
            try
            {
                EnclaveKost kost = analyse.EnclaveKosten
                    .SingleOrDefault(k => k.Id == id);
                if (kost != null)
                {
                    analyse.EnclaveKosten.Remove(kost);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();
                }
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "EnclaveKosten", "Verwijder"));
                _exceptionLogRepository.Save();
                TempData["error"] = Meldingen.VerwijderFoutmeldingKost;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Helpers
        private IEnumerable<EnclaveKostViewModel> MaakModel(Analyse analyse)
        {
            return analyse
                .EnclaveKosten
                .Select(m => new EnclaveKostViewModel(m))
                .ToList();
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.EnclaveKosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            decimal totaal = KostOfBaatExtensions.GeefTotaal(analyse.EnclaveKosten);

            ViewData["totaal"] = totaal.ToString("C", new CultureInfo("nl-BE"));
        }
        #endregion
    }
}