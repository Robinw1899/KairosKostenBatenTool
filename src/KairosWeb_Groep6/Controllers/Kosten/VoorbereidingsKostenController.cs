using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.ExtraKostViewModels;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.VoorbereidingsKostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers.Kosten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    public class VoorbereidingsKostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public VoorbereidingsKostenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            VoorbereidingsKostIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            PlaatsTotaalInViewData(analyse);

            return View(model);
        }

        public IActionResult VoegToe(Analyse analyse, VoorbereidingsKostIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                VoorbereidingsKost kost = new VoorbereidingsKost
                {
                    Type = model.Type,
                    Soort = model.Soort,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.VoorbereidingsKosten.Add(kost);
                _analyseRepository.Save();

                model = MaakModel(analyse);
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            VoorbereidingsKost kost = KostOfBaatExtensions.GetBy(analyse.VoorbereidingsKosten, id);

            VoorbereidingsKostIndexViewModel model = MaakModel(analyse);

            if (kost != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                model.Type = kost.Type;
                model.Soort = kost.Soort;
                model.Beschrijving = kost.Beschrijving;
                model.Bedrag = kost.Bedrag;
                model.ViewModels = analyse.VoorbereidingsKosten
                    .Select(m => new VoorbereidingsKostViewModel(m));
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, VoorbereidingsKostIndexViewModel model)
        {
            VoorbereidingsKost kost = KostOfBaatExtensions.GetBy(analyse.VoorbereidingsKosten, model.Id);

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

                return RedirectToAction("Index", model);
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            VoorbereidingsKost kost = KostOfBaatExtensions.GetBy(analyse.VoorbereidingsKosten, id);

            if (kost != null)
            {
                analyse.VoorbereidingsKosten.Remove(kost);
                _analyseRepository.Save();
            }

            VoorbereidingsKostIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De kost is succesvol verwijderd.";

            return View("Index", model);
        }

        private VoorbereidingsKostIndexViewModel MaakModel(Analyse analyse)
        {
            VoorbereidingsKostIndexViewModel model = new VoorbereidingsKostIndexViewModel()
            {
                Type = Type.Kost,
                Soort = Soort.ExtraKost,
                ViewModels = analyse
                                .VoorbereidingsKosten
                                .Select(m => new VoorbereidingsKostViewModel(m))
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.VoorbereidingsKosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            double totaal = KostOfBaatExtensions.GeefTotaal(analyse.VoorbereidingsKosten);

            ViewData["totaal"] = totaal.ToString("C");
        }
    }
}
