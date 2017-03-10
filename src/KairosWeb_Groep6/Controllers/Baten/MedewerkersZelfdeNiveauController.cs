﻿using System;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.KairosViewModels.Baten.MedewerkerNiveauBaatViewModels;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class MedewerkersZelfdeNiveauController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public MedewerkersZelfdeNiveauController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            MedewerkerNiveauIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, MedewerkerNiveauIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                MedewerkerNiveauBaat baat = new MedewerkerNiveauBaat
                {
                    //Id = model.Id,
                    Id = 1,
                    Type = model.Type,
                    Soort = model.Soort,
                    Uren = model.Uren,
                    BrutoMaandloonFulltime = model.BrutoMaandloonFulltime
                };

                analyse.MedewerkersZelfdeNiveauBaat.Add(baat);
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
            MedewerkerNiveauBaat baat = analyse.MedewerkersZelfdeNiveauBaat
                                                .SingleOrDefault(b => b.Id == id);

            MedewerkerNiveauIndexViewModel model = MaakModel(analyse);

            if (baat != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                model.Type = baat.Type;
                model.Soort = baat.Soort;
                model.Uren = baat.Uren;
                model.BrutoMaandloonFulltime = baat.BrutoMaandloonFulltime;
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, MedewerkerNiveauIndexViewModel model)
        {// id is het id van de baat die moet bewerkt worden
            MedewerkerNiveauBaat baat = analyse.MedewerkersZelfdeNiveauBaat
                                                 .SingleOrDefault(b => b.Id == model.Id);

            if (ModelState.IsValid && baat != null)
            {
                // parameters voor formulier instellen
                baat.Id = model.Id;
                baat.Type = model.Type;
                baat.Soort = model.Soort;
                baat.Uren = model.Uren;
                baat.BrutoMaandloonFulltime = model.BrutoMaandloonFulltime;
                _analyseRepository.Save();

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                return RedirectToAction("Index", model);
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            MedewerkerNiveauBaat baat = analyse.MedewerkersZelfdeNiveauBaat
                                                 .SingleOrDefault(b => b.Id == id);
            if (baat != null)
            {
                analyse.MedewerkersZelfdeNiveauBaat.Remove(baat);
                _analyseRepository.Save();
            }

            MedewerkerNiveauIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        private MedewerkerNiveauIndexViewModel MaakModel(Analyse analyse)
        {
            MedewerkerNiveauIndexViewModel model = new MedewerkerNiveauIndexViewModel
            {
                Type = Type.Baat,
                Soort = Soort.MedewerkersZelfdeNiveau,
                ViewModels = analyse
                                .MedewerkersZelfdeNiveauBaat
                                .Select(m => new MedewerkerNiveauBaatViewModel(m))
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.MedewerkersZelfdeNiveauBaat.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            double totaal = analyse.MedewerkersZelfdeNiveauBaat
                                    .Sum(t => t.Bedrag);

            ViewData["totaal"] = totaal.ToString("C");
        }
    }
}