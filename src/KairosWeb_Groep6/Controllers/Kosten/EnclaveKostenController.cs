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
    public class EnclaveKostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public EnclaveKostenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            IEnumerable<EnclaveKostViewModel> viewModels = MaakModel(analyse);

            PlaatsTotaalInViewData(analyse);

            return View(viewModels);
        }
        #endregion

        #region VoegToe
        public IActionResult VoegToe()
        {
            EnclaveKostViewModel model = new EnclaveKostViewModel();
            return PartialView("_Formulier", model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, EnclaveKostViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    EnclaveKost kost = new EnclaveKost
                    {
                        Soort = model.Soort,
                        Type = model.Type,
                        Beschrijving = model.Beschrijving,
                        Bedrag = model.Bedrag
                    };

                    analyse.EnclaveKosten.Add(kost);
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
        {// id is het id van de baat die moet bewerkt wordens
            try
            {
                EnclaveKost kost = KostOfBaatExtensions.GetBy(analyse.EnclaveKosten, id);
                EnclaveKostViewModel model = new EnclaveKostViewModel();

                if (kost != null)
                {
                    // parameters voor formulier instellen
                    model.Id = id;
                    model.Type = kost.Type;
                    model.Beschrijving = kost.Beschrijving;
                    model.Soort = kost.Soort;
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
        public IActionResult Bewerk(Analyse analyse, EnclaveKostViewModel model)
        {
            try
            {
                EnclaveKost kost = KostOfBaatExtensions.GetBy(analyse.EnclaveKosten, model.Id);
                
                if (ModelState.IsValid && kost != null)
                {
                    kost.Id = model.Id;
                    kost.Type = model.Type;
                    kost.Beschrijving = model.Beschrijving;
                    kost.Soort = model.Soort;
                    kost.Bedrag = model.Bedrag;

                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.OpslaanSuccesvolKost;
                }
            }
            catch
            {
                TempData["error"] = Meldingen.OpslaanFoutmeldingKost;
                return RedirectToAction("Index");
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
                EnclaveKost kost = analyse.EnclaveKosten
                                                 .SingleOrDefault(k => k.Id == id);
                if (kost != null)
                {
                    analyse.EnclaveKosten.Remove(kost);
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
        private IEnumerable<EnclaveKostViewModel> MaakModel(Analyse analyse)
        {
            return analyse
                .EnclaveKosten
                .Select(m => new EnclaveKostViewModel(m))
                .ToList();
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.EnclaveKosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }
            
            double totaal = KostOfBaatExtensions.GeefTotaal(analyse.EnclaveKosten);

            ViewData["totaal"] = totaal.ToString("C");
        }
        #endregion
    }
}

