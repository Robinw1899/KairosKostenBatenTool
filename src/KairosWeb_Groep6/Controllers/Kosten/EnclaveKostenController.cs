using System;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.EnclaveKostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers.Kosten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
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
                    Soort = model.Soort,
                    Type = model.Type,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.EnclaveKosten.Add(kost);
                analyse.DatumLaatsteAanpassing = DateTime.Now;
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De kost is succesvol toegevoegd.";
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            EnclaveKost kost = KostOfBaatExtensions.GetBy(analyse.EnclaveKosten, id);

            EnclaveKostenIndexViewModel model = MaakModel(analyse);

            if (kost != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                model.Type = kost.Type;
                model.Beschrijving = kost.Beschrijving;
                model.Soort = kost.Soort;
                model.Bedrag = kost.Bedrag;
                model.ToonFormulier = 1;
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, EnclaveKostenIndexViewModel model)
        {
            EnclaveKost kost = KostOfBaatExtensions.GetBy(analyse.EnclaveKosten, model.Id);

            if (ModelState.IsValid && kost != null)
            {
                // parameters voor formulier instellen
                kost.Id = model.Id;
                kost.Type = model.Type;
                kost.Beschrijving = model.Beschrijving;
                kost.Soort = model.Soort;
                kost.Bedrag = model.Bedrag;

                analyse.DatumLaatsteAanpassing = DateTime.Now;
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De kost is succesvol opgeslaan.";
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
                analyse.DatumLaatsteAanpassing = DateTime.Now;
                _analyseRepository.Save();
            }

            EnclaveKostenIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De kost is succesvol verwijderd.";

            return View("Index", model);
        }
        private EnclaveKostenIndexViewModel MaakModel(Analyse analyse)
        {
            EnclaveKostenIndexViewModel model = new EnclaveKostenIndexViewModel()
            {
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
            
            double totaal = KostOfBaatExtensions.GeefTotaal(analyse.EnclaveKosten);

            ViewData["totaal"] = totaal.ToString("C");
        }
    }
}

