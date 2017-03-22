using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.BegeleidingsKostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers.Kosten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    [ValidateAntiForgeryToken]
    public class BegeleidingsKostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public BegeleidingsKostenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            BegeleidingsKostenIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            PlaatsTotaalInViewData(analyse);

            return View(model);
        }

        public IActionResult VoegToe(Analyse analyse, BegeleidingsKostenIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                BegeleidingsKost kost = new BegeleidingsKost
                {
                    //Id = model.Id,
                    Id = 1,
                    Type = model.Type,
                    Soort = model.Soort,
                    Uren = model.Uren,
                    BrutoMaandloonBegeleider = model.BrutoMaandloonBegeleider
                };

                analyse.BegeleidingsKosten.Add(kost);
                _analyseRepository.Save();

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index", model);
        }

        [HttpGet]
        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            BegeleidingsKost kost = analyse.BegeleidingsKosten
                                                .SingleOrDefault(b => b.Id == id);

            BegeleidingsKostenIndexViewModel model = MaakModel(analyse);

            if (kost != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                model.Type = kost.Type;
                model.Soort = kost.Soort;
                model.Uren = kost.Uren;
                model.BrutoMaandloonBegeleider = kost.BrutoMaandloonBegeleider;
                model.ViewModels = analyse.BegeleidingsKosten
                    .Select(m => new BegeleidingsKostViewModel(m)
                    {
                        Bedrag = m.GeefJaarbedrag(analyse.Departement.Werkgever.PatronaleBijdrage)
                    });
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, BegeleidingsKostenIndexViewModel model)
        {// id is het id van de baat die moet bewerkt worden
            BegeleidingsKost kost = analyse.BegeleidingsKosten
                                                 .SingleOrDefault(b => b.Id == model.Id);

            if (ModelState.IsValid && kost != null)
            {
                // parameters voor formulier instellen
                kost.Id = model.Id;
                kost.Type = model.Type;
                kost.Soort = model.Soort;
                kost.Uren = model.Uren;
                kost.BrutoMaandloonBegeleider = model.BrutoMaandloonBegeleider;

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
            BegeleidingsKost baat = analyse.BegeleidingsKosten
                                                 .SingleOrDefault(b => b.Id == id);
            if (baat != null)
            {
                analyse.BegeleidingsKosten.Remove(baat);
                _analyseRepository.Save();
            }

            BegeleidingsKostenIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De kost is succesvol verwijderd.";

            return View("Index", model);
        }

        public IActionResult Info()
        {
            // Deze methode toont een pagina met de tabel van tabblad 4 van de Excel
            // = de toelichting van de begeleidingskosten
            return View();
        }

        private BegeleidingsKostenIndexViewModel MaakModel(Analyse analyse)
        {
            BegeleidingsKostenIndexViewModel model = new BegeleidingsKostenIndexViewModel
            {
                Type = Type.Kost,
                Soort = Soort.BegeleidingsKost,
                ViewModels = analyse
                                .BegeleidingsKosten
                                .Select(m => new BegeleidingsKostViewModel(m)
                                                {
                                                    Bedrag = m.GeefJaarbedrag(analyse.Departement.Werkgever.PatronaleBijdrage)
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
            if (analyse.BegeleidingsKosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            double totaal = BegeleidingsKostExtensions.GeefTotaal(analyse.BegeleidingsKosten, 
                                                                    analyse.Departement.Werkgever.PatronaleBijdrage);

            ViewData["totaal"] = totaal.ToString("C");
        }
    }
}
