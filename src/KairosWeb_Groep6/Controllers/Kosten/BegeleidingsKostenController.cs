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
    public class BegeleidingsKostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;

        public BegeleidingsKostenController(IAnalyseRepository analyseRepository,
            IExceptionLogRepository exceptionLogRepository)
        {
            _analyseRepository = analyseRepository;
            _exceptionLogRepository = exceptionLogRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId, Soort.BegeleidingsKost);
            analyse.UpdateTotalen(_analyseRepository);

            PlaatsTotaalInViewData(analyse);

            return View(MaakModel(analyse));
        }
        #endregion

        #region VoegToe
        public IActionResult VoegToe()
        {
            BegeleidingsKostViewModel model = new BegeleidingsKostViewModel();
            return PartialView("_Formulier", model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, BegeleidingsKostViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    analyse = _analyseRepository.GetById(analyse.AnalyseId, Soort.BegeleidingsKost);
                    DecimalConverter dc = new DecimalConverter();
                    BegeleidingsKost kost = new BegeleidingsKost
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Uren = model.Uren,
                        BrutoMaandloonBegeleider =dc.ConvertToDecimal( model.BrutoMaandloonBegeleider)
                    };
                    
                    analyse.BegeleidingsKosten.Add(kost);
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
                    _exceptionLogRepository.Add(new ExceptionLog(e, "BegeleidingsKost", "VoegToe -- POST --"));
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
                analyse = _analyseRepository.GetById(analyse.AnalyseId, Soort.BegeleidingsKost);
                BegeleidingsKost kost = KostOfBaatExtensions.GetBy(analyse.BegeleidingsKosten, id);
                BegeleidingsKostViewModel model = new BegeleidingsKostViewModel();
                DecimalConverter dc = new DecimalConverter();
                if (kost != null)
                {
                    // parameters voor formulier instellen
                    model.Id = id;
                    model.Type = kost.Type;
                    model.Soort = kost.Soort;
                    model.Uren = kost.Uren;
                    model.BrutoMaandloonBegeleider = dc.ConvertToString(kost.BrutoMaandloonBegeleider);

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
        public IActionResult Bewerk(Analyse analyse, BegeleidingsKostViewModel model)
        {// id is het id van de baat die moet bewerkt worden
            try
            {
                analyse = _analyseRepository.GetById(analyse.AnalyseId, Soort.BegeleidingsKost);
                BegeleidingsKost kost = KostOfBaatExtensions.GetBy(analyse.BegeleidingsKosten, model.Id);
                DecimalConverter dc = new DecimalConverter();
                if (ModelState.IsValid && kost != null)
                {
                    kost.Id = model.Id;
                    kost.Type = model.Type;
                    kost.Soort = model.Soort;
                    kost.Uren = model.Uren;
                    kost.BrutoMaandloonBegeleider = dc.ConvertToDecimal(model.BrutoMaandloonBegeleider);

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

            return RedirectToAction("Index");
        }
        #endregion

        #region Verwijder
        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            try
            {
                analyse = _analyseRepository.GetById(analyse.AnalyseId, Soort.BegeleidingsKost);
                BegeleidingsKost baat = KostOfBaatExtensions.GetBy(analyse.BegeleidingsKosten, id);
                if (baat != null)
                {
                    analyse.BegeleidingsKosten.Remove(baat);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();
                }
            }
            catch(Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "BegeleidingsKosten", "Verwijder"));
                _exceptionLogRepository.Save();
                TempData["error"] = Meldingen.VerwijderFoutmeldingKost;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Info --> Toelichting
        public IActionResult Info()
        {
            // Deze methode toont een pagina met de tabel van tabblad 4 van de Excel
            // = de toelichting van de begeleidingskosten
            return View();
        }
        #endregion

        #region Helpers
        private IEnumerable<BegeleidingsKostViewModel> MaakModel(Analyse analyse)
        {
            DecimalConverter dc = new DecimalConverter();
            return analyse
                .BegeleidingsKosten
                .Select(m => new BegeleidingsKostViewModel(m)
                {
                    Bedrag = analyse.Departement == null
                        ? ""
                        : dc.ConvertToString(m.GeefJaarbedrag(analyse.Departement.Werkgever.PatronaleBijdrage))
                })
                .ToList();
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.BegeleidingsKosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            if (analyse.Departement != null)
            {
                decimal totaal = BegeleidingsKostExtensions.GeefTotaal(analyse.BegeleidingsKosten,
                    analyse.Departement.Werkgever.PatronaleBijdrage);

                ViewData["totaal"] = totaal.ToString("C", new CultureInfo("nl-BE"));
            }
            else
            {
                ViewData["totaal"] = 0;
                TempData["error"] = "Opgelet! U heeft nog geen werkgever geselecteerd. Er zal dus nog geen resultaat " +
                                    "berekend worden bij deze kost.";
            }
        }
        #endregion
    }
}
