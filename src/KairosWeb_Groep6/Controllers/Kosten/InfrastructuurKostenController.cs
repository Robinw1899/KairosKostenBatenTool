using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;

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

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            IEnumerable<InfrastructuurKostViewModel> viewModels = MaakModel(analyse);

            PlaatsTotaalInViewData(analyse);

            return View(viewModels);
        }
        #endregion

        #region VoegToe
        public IActionResult VoegToe()
        {
            InfrastructuurKostViewModel model = new InfrastructuurKostViewModel();
            return PartialView("_Formulier", model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, InfrastructuurKostViewModel model)
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

                try
                {
                    analyse.InfrastructuurKosten.Add(kost);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.VoegToeSuccesvolKost;
                }
                catch
                {
                    TempData["error"] = Meldingen.VoegToeFoutmeldingKost;
                }
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index");
        }
        #endregion

        #region Bewerk
        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            try
            {
                InfrastructuurKost kost = KostOfBaatExtensions.GetBy(analyse.InfrastructuurKosten, id);

                InfrastructuurKostViewModel model = new InfrastructuurKostViewModel();

                if (kost != null)
                {
                    // parameters voor formulier instellen
                    model.Id = id;
                    model.Type = kost.Type;
                    model.Soort = kost.Soort;
                    model.Beschrijving = kost.Beschrijving;
                    model.Bedrag = kost.Bedrag;
                }

                return PartialView("_Formulier", model);
            }
            catch
            {
                TempData["error"] = Meldingen.OphalenFoutmeldingKost;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, InfrastructuurKostViewModel model)
        {
            try
            {
                InfrastructuurKost kost = KostOfBaatExtensions.GetBy(analyse.InfrastructuurKosten, model.Id);

                if (ModelState.IsValid && kost != null)
                {
                    kost.Id = model.Id;
                    kost.Type = model.Type;
                    kost.Soort = model.Soort;
                    kost.Beschrijving = model.Beschrijving;
                    kost.Bedrag = model.Bedrag;

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
                InfrastructuurKost kost = KostOfBaatExtensions.GetBy(analyse.InfrastructuurKosten, id);

                if (kost != null)
                {
                    analyse.InfrastructuurKosten.Remove(kost);
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
        private IEnumerable<InfrastructuurKostViewModel> MaakModel(Analyse analyse)
        {
            return analyse
                .InfrastructuurKosten
                .Select(m => new InfrastructuurKostViewModel(m))
                .ToList();
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
        #endregion
    }
}
