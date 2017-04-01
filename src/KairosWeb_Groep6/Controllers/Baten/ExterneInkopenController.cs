using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.KairosViewModels.Baten.ExterneInkoopViewModels;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class ExterneInkopenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public ExterneInkopenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            ExterneInkopenIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            return View(model);
        }

        public IActionResult VoegToe(Analyse analyse, ExterneInkopenIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                ExterneInkoop baat = new ExterneInkoop
                {
                    Type = model.Type,
                    Soort = model.Soort,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.ExterneInkopen.Add(baat);
                analyse.DatumLaatsteAanpassing = DateTime.Now;
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De baat is succesvol toegevoegd.";                
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            ExterneInkoop baat = KostOfBaatExtensions.GetBy(analyse.ExterneInkopen, id);

            ExterneInkopenIndexViewModel model = MaakModel(analyse);

            if (baat != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                model.Type = baat.Type;
                model.Soort = baat.Soort;
                model.Beschrijving = baat.Beschrijving;
                model.Bedrag = baat.Bedrag;
                model.ToonFormulier = 1;
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, ExterneInkopenIndexViewModel model)
        {// id is het id van de baat die moet bewerkt worden
            ExterneInkoop baat = KostOfBaatExtensions.GetBy(analyse.ExterneInkopen, model.Id);

            if (ModelState.IsValid && baat != null)
            {
                // parameters voor formulier instellen
                baat.Id = model.Id;
                baat.Type = model.Type;
                baat.Soort = model.Soort;
                baat.Beschrijving = model.Beschrijving;
                baat.Bedrag = model.Bedrag;

                analyse.DatumLaatsteAanpassing = DateTime.Now;
                _analyseRepository.Save();

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                TempData["message"] = "De baat is succesvol opgeslaan.";

                return RedirectToAction("Index", model);
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            ExterneInkoop baat = KostOfBaatExtensions.GetBy(analyse.ExterneInkopen, id);

            if (baat != null)
            {
                analyse.ExterneInkopen.Remove(baat);
                analyse.DatumLaatsteAanpassing = DateTime.Now;
                _analyseRepository.Save();
            }

            ExterneInkopenIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De baat is succesvol verwijderd.";

            return View("Index", model);
        }

        private ExterneInkopenIndexViewModel MaakModel(Analyse analyse)
        {
            ExterneInkopenIndexViewModel model = new ExterneInkopenIndexViewModel
            {
                Type = Type.Baat,
                Soort = Soort.ExterneInkoop,
                ViewModels = analyse
                                .ExterneInkopen
                                .Select(m => new ExterneInkoopViewModel(m))
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.ExterneInkopen.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            double totaal = analyse.ExterneInkopen
                                    .Sum(t => t.Bedrag);

            ViewData["totaal"] = totaal.ToString("C");
        }
    }
}
