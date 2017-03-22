using System;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.ExtraKostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Controllers.Kosten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    public class ExtraKostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public ExtraKostenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            ExtraKostenIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            PlaatsTotaalInViewData(analyse);

            return View(model);
        }
        public IActionResult VoegToe(Analyse analyse, ExtraKostenIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                ExtraKost kost = new ExtraKost
                {
                    Type = model.Type,
                    Soort = model.Soort,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.ExtraKosten.Add(kost);
                _analyseRepository.Save();

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                analyse.DatumLaatsteAanpassing = DateTime.Now;

                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            //ExtraKost kost = analyse.ExtraKosten
            //                                    .SingleOrDefault(b => b.Id == id);
            ExtraKost kost = KostOfBaatExtensions.GetBy(analyse.ExtraKosten, id);

            ExtraKostenIndexViewModel model = MaakModel(analyse);

            if (kost != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                model.Type = kost.Type;
                model.Soort = kost.Soort;
                model.Beschrijving = kost.Beschrijving;
                model.Bedrag = kost.Bedrag;
                model.ViewModels = analyse.ExtraKosten
                    .Select(m => new ExtraKostViewModel(m));
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, ExtraKostenIndexViewModel model)
        {
            ExtraKost kost = KostOfBaatExtensions.GetBy(analyse.ExtraKosten, model.Id);

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

                analyse.DatumLaatsteAanpassing = DateTime.Now;

                return RedirectToAction("Index", model);
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            ExtraKost kost = KostOfBaatExtensions.GetBy(analyse.ExtraKosten, id);

            if (kost != null)
            {
                analyse.ExtraKosten.Remove(kost);
                _analyseRepository.Save();
            }

            ExtraKostenIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De kost is succesvol verwijderd.";

            analyse.DatumLaatsteAanpassing = DateTime.Now;

            return View("Index", model);
        }

        private ExtraKostenIndexViewModel MaakModel(Analyse analyse)
        {
            ExtraKostenIndexViewModel model = new ExtraKostenIndexViewModel()
            {
                Type = Type.Kost,
                Soort = Soort.ExtraKost,
                ViewModels = analyse
                                .ExtraKosten
                                .Select(m => new ExtraKostViewModel(m))
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.ExtraKosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            double totaal = KostOfBaatExtensions.GeefTotaal(analyse.ExtraKosten);

            ViewData["totaal"] = totaal.ToString("C");
        }
    }
}
