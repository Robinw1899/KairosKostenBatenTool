using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers.Kosten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    [AutoValidateAntiforgeryToken]
    public class VoorbereidingsKostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public VoorbereidingsKostenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            PlaatsTotaalInViewData(analyse);

            return View(MaakModel(analyse));
        }
        #endregion

        #region VoegToe
        public IActionResult VoegToe()
        {
            VoorbereidingsKostViewModel model = new VoorbereidingsKostViewModel();
            return PartialView("_Formulier", model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, VoorbereidingsKostViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    DecimalConverter dc = new DecimalConverter();
                    VoorbereidingsKost kost = new VoorbereidingsKost
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Beschrijving = model.Beschrijving,
                        Bedrag = dc.ConvertToDecimal(model.Bedrag)
                    };

                    analyse.VoorbereidingsKosten.Add(kost);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.VoegToeSuccesvolKost;
                }
            }
            catch
            {
                TempData["error"] = Meldingen.VoegToeFoutmeldingKost;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Bewerk
        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            try
            {
                VoorbereidingsKost kost = KostOfBaatExtensions.GetBy(analyse.VoorbereidingsKosten, id);

                VoorbereidingsKostViewModel model = new VoorbereidingsKostViewModel();

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
        public IActionResult Bewerk(Analyse analyse, VoorbereidingsKostViewModel model)
        {
            try
            {
                VoorbereidingsKost kost = KostOfBaatExtensions.GetBy(analyse.VoorbereidingsKosten, model.Id);
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

            return RedirectToAction("Index");
        }
        #endregion

        #region Verwijder
        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            try
            {
                VoorbereidingsKost kost = KostOfBaatExtensions.GetBy(analyse.VoorbereidingsKosten, id);

                if (kost != null)
                {
                    analyse.VoorbereidingsKosten.Remove(kost);
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
        private IEnumerable<VoorbereidingsKostViewModel> MaakModel(Analyse analyse)
        {
            return analyse
                .VoorbereidingsKosten
                .Select(m => new VoorbereidingsKostViewModel(m))
                .ToList();
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.VoorbereidingsKosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            decimal totaal = KostOfBaatExtensions.GeefTotaal(analyse.VoorbereidingsKosten);

            ViewData["totaal"] = totaal.ToString("C", new CultureInfo("nl-BE"));
        }
        #endregion
    }
}
