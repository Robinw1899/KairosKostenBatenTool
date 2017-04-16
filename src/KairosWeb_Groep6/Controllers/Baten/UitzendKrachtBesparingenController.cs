using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    public class UitzendKrachtBesparingenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public UitzendKrachtBesparingenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            IEnumerable<UitzendKrachtBesparingViewModel> viewModels = MaakModel(analyse);

            PlaatsTotaalInViewData(analyse);

            return View(viewModels);
        }
        #endregion

        #region VoegToe
        public IActionResult VoegToe()
        {
            UitzendKrachtBesparingViewModel model = new UitzendKrachtBesparingViewModel();
            return PartialView("_Formulier", model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, UitzendKrachtBesparingViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UitzendKrachtBesparing baat = new UitzendKrachtBesparing
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Beschrijving = model.Beschrijving,
                        Bedrag = model.Bedrag
                    };

                    analyse.UitzendKrachtBesparingen.Add(baat);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.VoegToeSuccesvolBaat;
                }
            }
            catch
            {
                TempData["error"] = Meldingen.VoegToeFoutmeldingBaat;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Bewerk
        public IActionResult Bewerk(Analyse analyse, int id)
        {
            try
            {
                UitzendKrachtBesparing baat = KostOfBaatExtensions.GetBy(analyse.UitzendKrachtBesparingen, id);

                UitzendKrachtBesparingViewModel model = new UitzendKrachtBesparingViewModel();

                if (baat != null)
                {
                    model.Id = id;
                    model.Type = baat.Type;
                    model.Soort = baat.Soort;
                    model.Beschrijving = baat.Beschrijving;
                    model.Bedrag = baat.Bedrag;

                    return PartialView("_Formulier", model);
                }
            }
            catch
            {
                TempData["error"] = Meldingen.OphalenFoutmeldingBaat;
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, UitzendKrachtBesparingViewModel model)
        {
            try
            {
                UitzendKrachtBesparing baat = KostOfBaatExtensions.GetBy(analyse.UitzendKrachtBesparingen, model.Id);

                if (ModelState.IsValid && baat != null)
                {
                    baat.Id = model.Id;
                    baat.Type = model.Type;
                    baat.Soort = model.Soort;
                    baat.Beschrijving = model.Beschrijving;
                    baat.Bedrag = model.Bedrag;

                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();

                    TempData["message"] = Meldingen.OpslaanSuccesvolBaat;
                }
            }
            catch
            {
                TempData["error"] = Meldingen.OpslaanFoutmeldingBaat;
            }
            
            return RedirectToAction("Index");
        }
        #endregion

        #region Verwijder
        public IActionResult Verwijder(Analyse analyse, int id)
        {
            try
            {
                UitzendKrachtBesparing baat = KostOfBaatExtensions.GetBy(analyse.UitzendKrachtBesparingen, id);

                if (baat != null)
                {
                    analyse.UitzendKrachtBesparingen.Remove(baat);
                    analyse.DatumLaatsteAanpassing = DateTime.Now;
                    _analyseRepository.Save();
                }
            }
            catch
            {
                TempData["error"] = Meldingen.VerwijderFoutmeldingBaat;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Helpers
        private IEnumerable<UitzendKrachtBesparingViewModel> MaakModel(Analyse analyse)
        {
            return analyse
                .UitzendKrachtBesparingen
                .Select(m => new UitzendKrachtBesparingViewModel(m))
                .ToList();
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.UitzendKrachtBesparingen.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            decimal totaal = analyse.UitzendKrachtBesparingen
                                    .Sum(t => t.Bedrag);

            ViewData["totaal"] = totaal.ToString("C", new CultureInfo("nl-BE"));
        }
        #endregion
    }
}
