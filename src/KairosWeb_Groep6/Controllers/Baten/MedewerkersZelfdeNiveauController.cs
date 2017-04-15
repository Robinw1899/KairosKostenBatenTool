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
    public class MedewerkersZelfdeNiveauController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public MedewerkersZelfdeNiveauController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
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
                    MedewerkerNiveauBaat baat = new MedewerkerNiveauBaat
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Uren = model.Uren,
                        BrutoMaandloonFulltime = model.BrutoMaandloonFulltime
                    };

                    analyse.MedewerkersZelfdeNiveauBaat.Add(baat);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.VoegToeSuccesvolBaat;
                }
            }
            catch
            {
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
                MedewerkerNiveauBaat baat = KostOfBaatExtensions.GetBy(analyse.MedewerkersZelfdeNiveauBaat, id);
                MedewerkerNiveauBaatViewModel model = new MedewerkerNiveauBaatViewModel();

                if (baat != null)
                {
                    // parameters voor formulier instellen
                    model.Id = id;
                    model.Type = baat.Type;
                    model.Soort = baat.Soort;
                    model.Uren = baat.Uren;
                    model.BrutoMaandloonFulltime = baat.BrutoMaandloonFulltime;

                    return PartialView("_Formulier", model);
                }
            }
            catch
            {
                TempData["error"] = Meldingen.OphalenFoutmeldingBaat;
            }
            
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, MedewerkerNiveauBaatViewModel model)
        {// id is het id van de baat die moet bewerkt worden
            try
            {
                MedewerkerNiveauBaat baat = KostOfBaatExtensions.GetBy(analyse.MedewerkersZelfdeNiveauBaat, model.Id);

                if (ModelState.IsValid && baat != null)
                {
                    baat.Id = model.Id;
                    baat.Type = model.Type;
                    baat.Soort = model.Soort;
                    baat.Uren = model.Uren;
                    baat.BrutoMaandloonFulltime = model.BrutoMaandloonFulltime;

                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.OpslaanSuccesvolBaat;
                }
            }
            catch
            {
                TempData["error"] = Meldingen.OpslaanFoutmeldingBaat;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Verwijder
        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            try
            {
                MedewerkerNiveauBaat baat = KostOfBaatExtensions.GetBy(analyse.MedewerkersZelfdeNiveauBaat, id);

                if (baat != null)
                {
                    analyse.MedewerkersZelfdeNiveauBaat.Remove(baat);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();
                }
            }
            catch
            {
                TempData["error"] = Meldingen.VerwijderFoutmeldingBaat;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Helpers
        private IEnumerable<MedewerkerNiveauBaatViewModel> MaakModel(Analyse analyse)
        {
            return analyse
                .MedewerkersZelfdeNiveauBaat
                .Select(m => new MedewerkerNiveauBaatViewModel(m)
                {
                    Bedrag = analyse.Departement == null
                        ? 0
                        : m.BerekenTotaleLoonkostPerJaar(analyse.Departement.Werkgever.AantalWerkuren,
                            analyse.Departement.Werkgever.PatronaleBijdrage)
                })
                .ToList();
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.MedewerkersZelfdeNiveauBaat.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            if(analyse.Departement != null) { 
                double totaal = MedewerkerNiveauBaatExtensions.GeefTotaal(
                    analyse.MedewerkersZelfdeNiveauBaat,
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