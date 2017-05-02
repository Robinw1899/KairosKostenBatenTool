using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KairosWeb_Groep6.Filters;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;
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

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            IEnumerable<GereedschapsKostViewModel> viewModels = MaakModel(analyse);

            PlaatsTotaalInViewData(analyse);

            return View(viewModels);
        }
        #endregion

        #region VoegToe
        public IActionResult VoegToe()
        {
            GereedschapsKostViewModel model = new GereedschapsKostViewModel();
            return PartialView("_Formulier", model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, GereedschapsKostViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DecimalConverter dc = new DecimalConverter();
                    GereedschapsKost kost = new GereedschapsKost
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Beschrijving = model.Beschrijving,
                        Bedrag = dc.ConvertToDecimal(model.Bedrag)
                    };

                    analyse.GereedschapsKosten.Add(kost);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.VoegToeSuccesvolKost;
                }
            }
            catch
            {
                TempData["error"] = Meldingen.VoegToeFoutmeldingKost;
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index");
        }
        #endregion

        #region Bewerk
        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de kost die moet bewerkt worden
            try
            {
                GereedschapsKost kost = KostOfBaatExtensions.GetBy(analyse.GereedschapsKosten, id);
                GereedschapsKostViewModel model = new GereedschapsKostViewModel();
                DecimalConverter dc = new DecimalConverter();
                if (kost != null)
                {
                    // parameters voor formulier instellen
                    model.Id = id;
                    model.Type = kost.Type;
                    model.Soort = kost.Soort;
                    model.Beschrijving = kost.Beschrijving;
                    model.Bedrag = dc.ConvertToString(kost.Bedrag);

                    return PartialView("_Formulier", model);
                }
            }
            catch
            {
                TempData["error"] = Meldingen.OphalenFoutmeldingKost;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, GereedschapsKostViewModel model)
        {
            try
            {
                GereedschapsKost kost = KostOfBaatExtensions.GetBy(analyse.GereedschapsKosten, model.Id);
                DecimalConverter dc = new DecimalConverter();
                if (ModelState.IsValid && kost != null)
                {
                    kost.Id = model.Id;
                    kost.Type = model.Type;
                    kost.Soort = model.Soort;
                    kost.Beschrijving = model.Beschrijving;
                    kost.Bedrag = dc.ConvertToDecimal(model.Bedrag);

                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.OpslaanSuccesvolKost;
                }
            }
            catch
            {
                TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index");
        }
        #endregion

        #region Verwijder
        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            try
            {
                GereedschapsKost kost = KostOfBaatExtensions.GetBy(analyse.GereedschapsKosten, id);

                if (kost != null)
                {
                    analyse.GereedschapsKosten.Remove(kost);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();
                }
            }
            catch
            {
                TempData["error"] = Meldingen.VerwijderFoutmeldingKost;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Helpers
        private IEnumerable<GereedschapsKostViewModel> MaakModel(Analyse analyse)
        {
            return analyse
                .GereedschapsKosten
                .Select(m => new GereedschapsKostViewModel(m))
                .ToList();
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.GereedschapsKosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            decimal totaal = KostOfBaatExtensions.GeefTotaal(analyse.GereedschapsKosten);

            ViewData["totaal"] = totaal.ToString("C", new CultureInfo("nl-BE"));
        }
        #endregion
    }
}
