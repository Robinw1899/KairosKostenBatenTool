using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.EnclaveKostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Controllers.Kosten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    [ValidateAntiForgeryToken]
    public class EnclaveKostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public EnclaveKostenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            EnclaveKostenIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            PlaatsTotaalInViewData(analyse);

            return View(model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, EnclaveKostenIndexViewModel model)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            if (ModelState.IsValid)
            {
                EnclaveKost kost = new EnclaveKost
                {
                    Beschrijving = model.Beschrijving,
                    Soort = model.Soort,
                    Type = model.Type,     
                    Bedrag = model.Bedrag
                };

                analyse.EnclaveKosten.Add(kost);
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

            EnclaveKost kost = analyse.EnclaveKosten
                                              .SingleOrDefault(b => b.Id == id);

            EnclaveKostenIndexViewModel model = MaakModel(analyse);

            if (kost != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                //functie
                model.Type = kost.Type;
                model.Beschrijving = kost.Beschrijving;
                model.Soort = kost.Soort;
                model.Bedrag = kost.Bedrag;
             
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, EnclaveKostenIndexViewModel model)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            EnclaveKost kost = analyse.EnclaveKosten
                                             .SingleOrDefault(b => b.Id == model.Id);


            if (ModelState.IsValid && kost != null)
            {
                // parameters voor formulier instellen
                model.Id = kost.Id;
                //functie
                model.Type = kost.Type;
                model.Beschrijving = kost.Beschrijving;
                model.Soort = kost.Soort;
                model.Bedrag = kost.Bedrag;

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                return RedirectToAction("Index", model);
            }
            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de kost die moet verwijderd worden
            analyse = _analyseRepository.GetById(analyse.AnalyseId);
            EnclaveKost kost = analyse.EnclaveKosten
                                                 .SingleOrDefault(k => k.Id == id);
            if (kost != null)
            {
                analyse.EnclaveKosten.Remove(kost);
                _analyseRepository.Save();
            }

            EnclaveKostenIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De waarden zijn succesvol verwijderd.";

            return View("Index", model);
        }
        private EnclaveKostenIndexViewModel MaakModel(Analyse analyse)
        {
            EnclaveKostenIndexViewModel model = new EnclaveKostenIndexViewModel()
            {
                Type = Type.Baat,
                Soort = Soort.Loonkost,
                ViewModels = analyse
                                .EnclaveKosten
                                .Select(m => new EnclaveKostenViewModel(m))                
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.EnclaveKosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }
 
        }
    }
}

