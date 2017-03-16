﻿using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten.OverurenBesparingViewModels;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class OverurenBesparingController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public OverurenBesparingController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            OverurenBesparingIndexViewModel model = MaakModel(analyse);

            return View(model);
        }

        public IActionResult Opslaan(Analyse analyse, OverurenBesparingIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                OverurenBesparing baat = new OverurenBesparing
                {
                    //Id = model.Id,
                    Id = 1,
                    Type = model.Type,
                    Soort = model.Soort,
                    Bedrag = model.Bedrag
                };

                analyse.OverurenBesparing = baat;
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De waarden zijn succesvol opgeslagen.";
            }

            return RedirectToAction("Index", model);
        }

        private OverurenBesparingIndexViewModel MaakModel(Analyse analyse)
        {
            if (analyse.OverurenBesparing == null)
            {
                analyse.OverurenBesparing = new OverurenBesparing();
            }

            return new OverurenBesparingIndexViewModel(analyse.OverurenBesparing);
        }
    }
}