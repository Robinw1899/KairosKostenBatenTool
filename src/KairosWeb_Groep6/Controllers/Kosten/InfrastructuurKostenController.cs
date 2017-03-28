using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.InfrastructuurKostenViewModels;

namespace KairosWeb_Groep6.Controllers.Kosten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    public class InfrastructuurKostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public InfrastructuurKostenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            InfrastructuurKostenIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            return View(model);
        }

        public IActionResult VoegToe(Analyse analyse, InfrastructuurKostenIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                InfrastructuurKost kost = new InfrastructuurKost
                {
                    Type = model.Type,
                    Soort = model.Soort,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.InfrastructuurKosten.Add(kost);
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De kost is succesvol toegevoegd.";
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            InfrastructuurKost kost = KostOfBaatExtensions.GetBy(analyse.InfrastructuurKosten, id);

            InfrastructuurKostenIndexViewModel model = MaakModel(analyse);

            if (kost != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                model.Type = kost.Type;
                model.Soort = kost.Soort;
                model.Beschrijving = kost.Beschrijving;
                model.Bedrag = kost.Bedrag;
                model.ViewModels = analyse.InfrastructuurKosten
                                            .Select(m => new InfrastructuurKostenViewModel(m));
                model.ToonFormulier = 1;
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, InfrastructuurKostenIndexViewModel model)
        {
            InfrastructuurKost kost = KostOfBaatExtensions.GetBy(analyse.InfrastructuurKosten, model.Id);

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

                return View("Index", model);
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            InfrastructuurKost kost = KostOfBaatExtensions.GetBy(analyse.InfrastructuurKosten, id);

            if (kost != null)
            {
                analyse.InfrastructuurKosten.Remove(kost);
                _analyseRepository.Save();
            }

            InfrastructuurKostenIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De kost is succesvol verwijderd.";

            return View("Index", model);
        }

        private InfrastructuurKostenIndexViewModel MaakModel(Analyse analyse)
        {
            InfrastructuurKostenIndexViewModel model = new InfrastructuurKostenIndexViewModel()
            {
                Type = Type.Kost,
                Soort = Soort.InfrastructuurKost,
                ViewModels = analyse
                                .InfrastructuurKosten
                                .Select(m => new InfrastructuurKostenViewModel(m))
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.InfrastructuurKosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            double totaal = KostOfBaatExtensions.GeefTotaal(analyse.InfrastructuurKosten);

            ViewData["totaal"] = totaal.ToString("C");
        }
    }
}
