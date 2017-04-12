using System;
using System.Collections.Generic;
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
    public class ExtraKostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public ExtraKostenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            IEnumerable<ExtraKostViewModel> viewModels = MaakModel(analyse);

            PlaatsTotaalInViewData(analyse);

            return View(viewModels);
        }
        #endregion

        #region VoegToe
        public IActionResult VoegToe()
        {
            ExtraKostViewModel model = new ExtraKostViewModel();
            return PartialView("_Formulier", model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, ExtraKostViewModel model)
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

                try
                {
                    analyse.ExtraKosten.Add(kost);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = "De kost is succesvol toegevoegd.";
                }
                catch
                {
                    TempData["error"] = "Er ging iets mis, probeer later opnieuw";
                }
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index");
        }
        #endregion

        #region Bewerk
        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de kost die moet bewerkt wordens
            try
            {
                ExtraKost kost = KostOfBaatExtensions.GetBy(analyse.ExtraKosten, id);
                ExtraKostViewModel model = new ExtraKostViewModel();

                // parameters voor formulier instellen
                if (kost != null)
                {
                    model.Id = id;
                    model.Type = kost.Type;
                    model.Soort = kost.Soort;
                    model.Beschrijving = kost.Beschrijving;
                    model.Bedrag = kost.Bedrag;

                    return PartialView("_Formulier", model);
                }
            }
            catch
            {
                TempData["error"] = "Er ging iets mis, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, ExtraKostViewModel model)
        {
            try
            {
                ExtraKost kost = KostOfBaatExtensions.GetBy(analyse.ExtraKosten, model.Id);

                if (ModelState.IsValid && kost != null)
                {
                    kost.Id = model.Id;
                    kost.Type = model.Type;
                    kost.Soort = model.Soort;
                    kost.Beschrijving = model.Beschrijving;
                    kost.Bedrag = model.Bedrag;

                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = "De kost is succesvol opgeslaan.";
                }
            }
            catch
            {
                TempData["error"] = "Er ging iets mis, probeer later opnieuw";
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index");
        }
        #endregion

        #region Verwijder
        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de kost die moet verwijderd worden
            try
            {
                ExtraKost kost = KostOfBaatExtensions.GetBy(analyse.ExtraKosten, id);

                if (kost != null)
                {
                    analyse.ExtraKosten.Remove(kost);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();
                }
            }
            catch
            {
                TempData["error"] = "Er ging iets mis, probeer later opnieuw";
                PlaatsTotaalInViewData(analyse);
                return RedirectToAction("Index");
            }
            
            PlaatsTotaalInViewData(analyse);

            return PartialView("_OverzichtTabel", MaakModel(analyse));
        }
        #endregion

        #region Helpers
        private IEnumerable<ExtraKostViewModel> MaakModel(Analyse analyse)
        {
            return analyse
                .ExtraKosten
                .Select(m => new ExtraKostViewModel(m))
                .ToList();
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
        #endregion
    }
}
