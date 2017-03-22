using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.GereedschapsKostenViewModels;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KairosWeb_Groep6.Controllers.Kosten
{
    public class GereedschapsKostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public GereedschapsKostenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            GereedschapsKostenIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            return View(model);
        }

        public IActionResult VoegToe(Analyse analyse, GereedschapsKostenIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                GereedschapsKost kost = new GereedschapsKost
                {
                    //Id = model.Id,
                    Id = 1,
                    Type = model.Type,
                    Soort = model.Soort,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.GereedschapsKosten.Add(kost);
                _analyseRepository.Save();

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                TempData["message"] = $"{model.Beschrijving} is succesvol opgeslagen.";

                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            /* PlaatsTotaalInViewData(analyse);*/

            return RedirectToAction("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            GereedschapsKost kost = analyse.GereedschapsKosten
                                                .SingleOrDefault(b => b.Id == id);

            GereedschapsKostenIndexViewModel model = MaakModel(analyse);

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
        public IActionResult Bewerk(Analyse analyse, GereedschapsKostenIndexViewModel model)
        {// id is het id van de baat die moet bewerkt worden
            OpleidingsKost kost = analyse.OpleidingsKosten
                                                 .SingleOrDefault(b => b.Id == model.Id);

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

                TempData["message"] = $"{model.Beschrijving} is succesvol opgeslagen.";

                return RedirectToAction("Index", model);
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            GereedschapsKost kost = analyse.GereedschapsKosten
                                                 .SingleOrDefault(b => b.Id == id);
            if (kost != null)
            {
                analyse.GereedschapsKosten.Remove(kost);
                _analyseRepository.Save();
            }

            GereedschapsKostenIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = $"{model.Beschrijving} is succesvol verwijderd.";

            return View("Index", model);
        }

        private GereedschapsKostenIndexViewModel MaakModel(Analyse analyse)
        {
            GereedschapsKostenIndexViewModel model = new GereedschapsKostenIndexViewModel()
            {
                Type = Models.Domain.Type.Kost,
                Soort = Soort.GereedschapsKost,
                ViewModels = analyse
                                .GereedschapsKosten
                                .Select(m => new GereedschapsKostenViewModel(m))
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.GereedschapsKosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            double totaal = analyse.GereedschapsKosten
                                    .Sum(t => t.Bedrag);

            ViewData["totaal"] = totaal.ToString("C");
        }
    }
}
