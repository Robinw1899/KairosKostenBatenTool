﻿using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Controllers.Kosten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    public class LoonkostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public LoonkostenController(IAnalyseRepository analyseRepository, IJobcoachRepository jobcoachRepository)
        {
            _analyseRepository = analyseRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            PlaatsTotaalInViewData(analyse);

            return View(MaakModel(analyse));
        }
        #endregion

        #region VoegToe
        public IActionResult VoegToe()
        {
            LoonkostViewModel model = new LoonkostViewModel();
            return PartialView("_Formulier", model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, Jobcoach jobcoach, LoonkostViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Loonkost kost = new Loonkost
                    {
                        Beschrijving = model.Beschrijving,
                        AantalUrenPerWeek = model.AantalUrenPerWeek,
                        BrutoMaandloonFulltime = model.BrutoMaandloonFulltime,
                        Doelgroep = model.Doelgroep,
                        Ondersteuningspremie = model.Ondersteuningspremie,
                        AantalMaandenIBO = model.AantalMaandenIBO,
                        IBOPremie = model.IBOPremie
                    };

                    analyse.Loonkosten.Add(kost);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.VoegToeSuccesvolKost;
                }
            }
            catch
            {
                TempData["error"] = Meldingen.VoegToeFoutmeldingKost;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Bewerk
        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            try
            {
                Loonkost kost = KostOfBaatExtensions.GetBy(analyse.Loonkosten, id);

                LoonkostViewModel model = new LoonkostViewModel();

                if (kost != null)
                {
                    // parameters voor formulier instellen
                    model.Id = id;
                    model.Beschrijving = kost.Beschrijving;
                    model.AantalUrenPerWeek = kost.AantalUrenPerWeek;
                    model.BrutoMaandloonFulltime = kost.BrutoMaandloonFulltime;
                    model.Doelgroep = kost.Doelgroep;
                    model.Ondersteuningspremie = kost.Ondersteuningspremie;
                    model.AantalMaandenIBO = kost.AantalMaandenIBO;
                    model.IBOPremie = kost.IBOPremie;

                    return PartialView("_Formulier", model);
                }
            }
            catch
            {
                TempData["error"] = Meldingen.OphalenFoutmeldingKost;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, LoonkostViewModel model)
        {
            try
            {
                Loonkost kost = KostOfBaatExtensions.GetBy(analyse.Loonkosten, model.Id);

                if (ModelState.IsValid && kost != null)
                {
                    kost.Id = model.Id;
                    kost.Beschrijving = model.Beschrijving;
                    kost.AantalUrenPerWeek = model.AantalUrenPerWeek;
                    kost.BrutoMaandloonFulltime = model.BrutoMaandloonFulltime;
                    kost.Doelgroep = model.Doelgroep;
                    kost.Ondersteuningspremie = model.Ondersteuningspremie;
                    kost.AantalMaandenIBO = model.AantalMaandenIBO;
                    kost.IBOPremie = model.IBOPremie;

                    if (kost.Doelgroep == null)
                    {
                        TempData["error"] =
                            "Opgelet! U heeft nog geen doelgroep geselecteerd. Er zal dus nog geen resultaat " +
                            "berekend worden bij deze kost.";
                    }

                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.OpslaanSuccesvolKost;
                }
            }
            catch
            {
                TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Verwijder
        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            try
            {
                Loonkost kost = KostOfBaatExtensions.GetBy(analyse.Loonkosten, id);

                if (kost != null)
                {
                    analyse.Loonkosten.Remove(kost);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();
                }
            }
            catch
            {
                TempData["error"] = Meldingen.VerwijderFoutmeldingKost;
                return RedirectToAction("Index");
            }

            PlaatsTotaalInViewData(analyse);

            return PartialView("_OverzichtTabel", MaakModel(analyse));
        }
        #endregion

        #region Helpers
        private IEnumerable<LoonkostViewModel> MaakModel(Analyse analyse)
        {
            return analyse
                .Loonkosten
                .Select(m => new LoonkostViewModel(m)
                {
                    Bedrag = analyse.Departement == null
                        ? 0
                        : m.BerekenTotaleLoonkost(analyse.Departement.Werkgever.AantalWerkuren,
                            analyse.Departement.Werkgever.PatronaleBijdrage)
                })
                .ToList();
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.Loonkosten.Count == 0)
            {
                ViewData["totaalBrutolonen"] = 0;
                ViewData["totaalLoonkosten"] = 0;
            }

            if (analyse.Departement != null)
            {
                double totaal = LoonkostExtensions.GeefTotaalBrutolonenPerJaarAlleLoonkosten(
                    analyse.Loonkosten, analyse.Departement.Werkgever.AantalWerkuren,
                    analyse.Departement.Werkgever.PatronaleBijdrage);

                ViewData["totaalBrutolonen"] = totaal.ToString("C");

                totaal = LoonkostExtensions.GeefTotaalAlleLoonkosten(
                    analyse.Loonkosten, analyse.Departement.Werkgever.AantalWerkuren,
                    analyse.Departement.Werkgever.PatronaleBijdrage);

                ViewData["totaalLoonkosten"] = totaal.ToString("C");
            }
            else
            {
                ViewData["totaalBrutolonen"] = 0;
                ViewData["totaalLoonkosten"] = 0;
                TempData["error"] = "Opgelet! U heeft nog geen werkgever geselecteerd. Er zal dus nog geen resultaat " +
                                    "berekend worden bij deze kost.";
            }
        }
        #endregion
    }
}

