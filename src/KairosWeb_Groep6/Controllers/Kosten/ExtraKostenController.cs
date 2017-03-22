using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.ExtraKostViewModels;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.LoonKostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Controllers.Kosten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    [ValidateAntiForgeryToken]
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

            LoonkostenIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            PlaatsTotaalInViewData(analyse);

            return View(model);
        }

        private ExtraKostenIndexViewModel MaakModel(Analyse analyse)
        {
            Array values = Enum.GetValues(typeof(Doelgroep));
            IList<Doelgroep> doelgroepen = new List<Doelgroep>();

            foreach (Doelgroep value in values)
            {
                doelgroepen.Add(value);
            }

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
            if (analyse.Loonkosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }


            double totaal = LoonkostExtensions.GeefTotaalBrutolonenPerJaarAlleLoonkosten(
                analyse.Loonkosten, analyse.Departement.Werkgever.AantalWerkuren, analyse.Departement.Werkgever.PatronaleBijdrage);

            ViewData["totaalBrutolonen"] = totaal.ToString("C");

            totaal = LoonkostExtensions.GeefTotaalAlleLoonkosten(
                 analyse.Loonkosten, analyse.Departement.Werkgever.AantalWerkuren, analyse.Departement.Werkgever.PatronaleBijdrage);

            ViewData["totaalLoonkosten"] = totaal.ToString("C");
        }
    }
}
