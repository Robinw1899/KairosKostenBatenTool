using System.Linq;
using KairosWeb_Groep6.Filters;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.GereedschapsKostenViewModels;
using Microsoft.AspNetCore.Authorization;

namespace KairosWeb_Groep6.Controllers.Kosten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
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
                GereedschapsKost kost = new GereedschapsKost
                {
                    Type = model.Type,
                    Soort = model.Soort,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.GereedschapsKosten.Add(kost);
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De kost is succesvol toegevoegd.";
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            GereedschapsKost kost = KostOfBaatExtensions.GetBy(analyse.GereedschapsKosten, id);

            GereedschapsKostenIndexViewModel model = MaakModel(analyse);

            if (kost != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                model.Type = kost.Type;
                model.Soort = kost.Soort;
                model.Beschrijving = kost.Beschrijving;
                model.Bedrag = kost.Bedrag;
                model.ViewModels = analyse.GereedschapsKosten
                                            .Select(m => new GereedschapsKostenViewModel(m));
                model.ToonFormulier = 1;
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, GereedschapsKostenIndexViewModel model)
        {
            GereedschapsKost kost = KostOfBaatExtensions.GetBy(analyse.GereedschapsKosten, model.Id);

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

                TempData["message"] = "De kost is succesvol opgeslaan.";
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            GereedschapsKost kost = KostOfBaatExtensions.GetBy(analyse.GereedschapsKosten, id);

            if (kost != null)
            {
                analyse.GereedschapsKosten.Remove(kost);
                _analyseRepository.Save();
            }

            GereedschapsKostenIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De kost is succesvol verwijderd.";

            return View("Index", model);
        }

        private GereedschapsKostenIndexViewModel MaakModel(Analyse analyse)
        {
            GereedschapsKostenIndexViewModel model = new GereedschapsKostenIndexViewModel()
            {
                Type = Type.Kost,
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

            double totaal = KostOfBaatExtensions.GeefTotaal(analyse.GereedschapsKosten);

            ViewData["totaal"] = totaal.ToString("C");
        }
    }
}
