﻿using System;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using Microsoft.AspNetCore.Authorization;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    public class SubsidieController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public SubsidieController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        #region Index
        public IActionResult Index(Analyse analyse)
        {
            SubsidieViewModel model = MaakModel(analyse);

            return View(model);
        }
        #endregion

        #region Opslaan
        [HttpPost]
        public IActionResult Opslaan(Analyse analyse, SubsidieViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // de baat bestaat reeds:
                    Subsidie baat = new Subsidie
                    {
                        Type = model.Type,
                        Soort = model.Soort,
                        Bedrag = model.Bedrag
                    };

                    analyse.Subsidie = baat;
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
        public IActionResult Verwijder(Analyse analyse)
        {
            try
            {
                // Baat eruit halen
                analyse.Subsidie = null;

                // Datum updaten
                analyse.DatumLaatsteAanpassing = DateTime.Now;

                // Opslaan
                _analyseRepository.Save();

                TempData["message"] = Meldingen.VerwijderSuccesvolBaat;
            }
            catch
            {
                TempData["error"] = Meldingen.VerwijderFoutmeldingBaat;
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Helpers
        private SubsidieViewModel MaakModel(Analyse analyse)
        {
            if (analyse.Subsidie == null)
            {
                return new SubsidieViewModel();
            }

            return new SubsidieViewModel(analyse.Subsidie);
        }
        #endregion
    }
}
