﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    [AutoValidateAntiforgeryToken]
    public class MedewerkersHogerNiveauController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;

        public MedewerkersHogerNiveauController(IAnalyseRepository analyseRepository,
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

            IEnumerable<MedewerkerNiveauBaatViewModel> viewModels = MaakModel(analyse);

            PlaatsTotaalInViewData(analyse);

            return View(viewModels);
        }
        #endregion

        #region VoegToe
        public IActionResult VoegToe()
        {
            MedewerkerNiveauBaatViewModel model = new MedewerkerNiveauBaatViewModel();
            return PartialView("_Formulier", model);
        }
        
        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, MedewerkerNiveauBaatViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DecimalConverter dc = new DecimalConverter();
                    MedewerkerNiveauBaat baat = new MedewerkerNiveauBaat
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Uren = model.Uren,
                        BrutoMaandloonFulltime = dc.ConvertToDecimal(model.BrutoMaandloonFulltime)
                    };

                    analyse.MedewerkersHogerNiveauBaat.Add(baat);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.VoegToeSuccesvolBaat;
                }
            }
            catch(Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "MedewerkersHogerNiveau", "VoegToe -- POST --"));
                _exceptionLogRepository.Save();
                TempData["error"] = Meldingen.VoegToeFoutmeldingBaat;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Bewerk
        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            try
            {
                MedewerkerNiveauBaat baat = KostOfBaatExtensions.GetBy(analyse.MedewerkersHogerNiveauBaat, id);
                MedewerkerNiveauBaatViewModel model = new MedewerkerNiveauBaatViewModel();
                DecimalConverter dc = new DecimalConverter();

                if (baat != null)
                {
                    // parameters voor formulier instellen
                    model.Id = id;
                    model.Type = baat.Type;
                    model.Soort = baat.Soort;
                    model.Uren = baat.Uren;
                    model.BrutoMaandloonFulltime = dc.ConvertToString(baat.BrutoMaandloonFulltime);

                    return PartialView("_Formulier", model);
                }
            }
            catch(Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "MedewerkersHogerNiveau", "Bewerk -- GET --"));
                _exceptionLogRepository.Save();
                TempData["error"] = Meldingen.OphalenFoutmeldingBaat;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, MedewerkerNiveauBaatViewModel model)
        {// id is het id van de baat die moet bewerkt worden
            try
            {
                MedewerkerNiveauBaat baat = KostOfBaatExtensions.GetBy(analyse.MedewerkersHogerNiveauBaat, model.Id);
                DecimalConverter dc = new DecimalConverter();
                if (ModelState.IsValid && baat != null)
                {
                    baat.Id = model.Id;
                    baat.Type = model.Type;
                    baat.Soort = model.Soort;
                    baat.Uren = model.Uren;
                    baat.BrutoMaandloonFulltime = dc.ConvertToDecimal(model.BrutoMaandloonFulltime);

                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.OpslaanSuccesvolBaat;
                }
            }
            catch(Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "MedewerkersHogerNiveau", "Bewerk -- POST --"));
                _exceptionLogRepository.Save();
                TempData["error"] = Meldingen.OphalenFoutmeldingKost;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Verwijder
        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            try
            {
                MedewerkerNiveauBaat baat = KostOfBaatExtensions.GetBy(analyse.MedewerkersHogerNiveauBaat, id);

                if (baat != null)
                {
                    analyse.MedewerkersHogerNiveauBaat.Remove(baat);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();
                }
            }
            catch(Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "MedewerkersHogerNiveau", "Verwijder"));
                _exceptionLogRepository.Save();
                TempData["error"] = Meldingen.VerwijderFoutmeldingBaat;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Helpers
        private IEnumerable<MedewerkerNiveauBaatViewModel> MaakModel(Analyse analyse)
        {
            DecimalConverter dc = new DecimalConverter();
            return analyse
                .MedewerkersHogerNiveauBaat
                .Select(m => new MedewerkerNiveauBaatViewModel(m)
                {
                    Bedrag = analyse.Departement == null
                        ? ""
                        :dc.ConvertToString(m.BerekenTotaleLoonkostPerJaar(analyse.Departement.Werkgever.AantalWerkuren,
                            analyse.Departement.Werkgever.PatronaleBijdrage))
                })
                .ToList();
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.MedewerkersHogerNiveauBaat.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            if(analyse.Departement != null) {
                decimal totaal = MedewerkerNiveauBaatExtensions.GeefTotaal(
                    analyse.MedewerkersHogerNiveauBaat,
                    analyse.Departement.Werkgever.AantalWerkuren,
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
