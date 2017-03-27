﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.OpleidingsKosten;
using Microsoft.AspNetCore.Mvc;


// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KairosWeb_Groep6.Controllers.Kosten
{
    public class OpleidingsKostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public OpleidingsKostenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            OpleidingsKostIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            return View(model);
        }

        public IActionResult VoegToe(Analyse analyse, OpleidingsKostIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                OpleidingsKost kost = new OpleidingsKost
                {
                    //Id = model.Id,
                    Id = 1,
                    Type = model.Type,
                    Soort = model.Soort,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.OpleidingsKosten.Add(kost);
                _analyseRepository.Save();

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                TempData["message"] = "De kost is succesvol opgeslaan.";

                return View("Index", model);
            }

            /* PlaatsTotaalInViewData(analyse);*/

            return RedirectToAction("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            OpleidingsKost kost = KostOfBaatExtensions.GetBy(analyse.OpleidingsKosten, id);

            OpleidingsKostIndexViewModel model = MaakModel(analyse);

            if (kost != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                model.Type = kost.Type;
                model.Soort = kost.Soort;
                model.Beschrijving = kost.Beschrijving;
                model.Bedrag = kost.Bedrag;
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, OpleidingsKostIndexViewModel model)
        {// id is het id van de baat die moet bewerkt worden
            OpleidingsKost kost = KostOfBaatExtensions.GetBy(analyse.OpleidingsKosten, model.Id);

            if (ModelState.IsValid && kost != null)
            {
                // parameters voor formulier instellen
                kost.Id = model.Id;
                kost.Type = model.Type;
                kost.Soort = model.Soort;
                kost.Beschrijving = model.Beschrijving;
                kost.Bedrag = model.Bedrag;

                _analyseRepository.Save();

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                TempData["message"] = "De kost is succesvol opgeslaan.";

                return RedirectToAction("Index", model);
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            OpleidingsKost kost = KostOfBaatExtensions.GetBy(analyse.OpleidingsKosten, id);
            if (kost != null)
            {
                analyse.OpleidingsKosten.Remove(kost);
                _analyseRepository.Save();
            }

            OpleidingsKostIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De kost is succesvol verwijderd.";

            return View("Index", model);
        }

        private OpleidingsKostIndexViewModel MaakModel(Analyse analyse)
        {
            OpleidingsKostIndexViewModel model = new OpleidingsKostIndexViewModel()
            {
                Type = Type.Kost,
                Soort = Soort.OpleidingsKost,
                ViewModels = analyse
                                .OpleidingsKosten
                                .Select(m => new OpleidingsKostViewModel(m))
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.OpleidingsKosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            double totaal = analyse.OpleidingsKosten
                                    .Sum(t => t.Bedrag);

            ViewData["totaal"] = totaal.ToString("C");
        }
    }
}