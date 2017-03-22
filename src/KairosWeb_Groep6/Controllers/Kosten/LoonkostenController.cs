﻿using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.LoonKostViewModels;
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

        public LoonkostenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            LoonkostenIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            PlaatsTotaalInViewData(analyse);

            return View(model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, LoonkostenIndexViewModel model)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

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
                _analyseRepository.Save();

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            Loonkost kost = analyse.Loonkosten
                                              .SingleOrDefault(b => b.Id == id);

            LoonkostenIndexViewModel model = MaakModel(analyse);

            if (kost != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                //functie
                model.AantalUrenPerWeek = kost.AantalUrenPerWeek;
                model.BrutoMaandloonFulltime = kost.BrutoMaandloonFulltime;
                model.Doelgroep = kost.Doelgroep;
                model.Ondersteuningspremie = kost.Ondersteuningspremie;
                model.AantalMaandenIBO = kost.AantalMaandenIBO;
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, LoonkostenIndexViewModel model)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            Loonkost kost = analyse.Loonkosten
                                             .SingleOrDefault(b => b.Id == model.Id);

           
            if (ModelState.IsValid && kost != null)
            {
                // parameters voor formulier instellen
                model.Id = kost.Id;
                //functie
                model.AantalUrenPerWeek = kost.AantalUrenPerWeek;
                model.BrutoMaandloonFulltime = kost.BrutoMaandloonFulltime;
                model.Doelgroep = kost.Doelgroep;
                model.Ondersteuningspremie = kost.Ondersteuningspremie;
                model.AantalMaandenIBO = kost.AantalMaandenIBO;

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                return RedirectToAction("Index", model);
            }
            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            analyse = _analyseRepository.GetById(analyse.AnalyseId);
            Loonkost kost = analyse.Loonkosten
                                                 .SingleOrDefault(k => k.Id == id);
            if (kost != null)
            {
                analyse.Loonkosten.Remove(kost);
                _analyseRepository.Save();
            }

            LoonkostenIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De waarden zijn succesvol verwijderd.";

            return View("Index", model);
        }
        private LoonkostenIndexViewModel MaakModel(Analyse analyse)
        {
            Array values = Enum.GetValues(typeof(Doelgroep));
            IList<Doelgroep> doelgroepen = new List<Doelgroep>();

            foreach (Doelgroep value in values)
            {
                doelgroepen.Add(value);
            }

            LoonkostenIndexViewModel model = new LoonkostenIndexViewModel(doelgroepen , Doelgroep.Andere)
            {
                Type = Type.Kost,
                Soort = Soort.Loonkost,
                ViewModels = analyse
                                .Loonkosten
                                .Select(m => new LoonkostViewModel(m)
                    {
                        Bedrag = m.BerekenTotaleLoonkost(analyse.Departement.Werkgever.AantalWerkuren, analyse.Departement.Werkgever.PatronaleBijdrage)
                    })
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.Loonkosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }


            double totaal = LoonkostExtensions.GeefTotaalBrutolonenPerJaarAlleLoonkosten(
                analyse.Loonkosten, analyse.Departement.Werkgever.AantalWerkuren, analyse.Departement.Werkgever.PatronaleBijdrage);

            ViewData["totaalBrutolonen"] = totaal.ToString("C");

            totaal = LoonkostExtensions.GeefTotaalAlleLoonkosten(
                 analyse.Loonkosten, analyse.Departement.Werkgever.AantalWerkuren, analyse.Departement.Werkgever.PatronaleBijdrage);

            ViewData["totaalLoonkosten"] = totaal.ToString("C");
        }
    }
}

