using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.KairosViewModels.Baten.UitzendKrachtBesparingViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class UitzendKrachtBesparingenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public UitzendKrachtBesparingenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            UitzendKrachtBesparingIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            return View(model);
        }

        public IActionResult VoegToe(Analyse analyse, UitzendKrachtBesparingIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                UitzendKrachtBesparing baat = new UitzendKrachtBesparing
                {
                    Type = model.Type,
                    Soort = model.Soort,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.UitzendKrachtBesparingen.Add(baat);
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De baat is succesvol toegevoegd.";
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {
            UitzendKrachtBesparing baat = KostOfBaatExtensions.GetBy(analyse.UitzendKrachtBesparingen, id);

            UitzendKrachtBesparingIndexViewModel model = MaakModel(analyse);

            if (baat != null)
            {
                // gegevens analyse die bewerkt wordt invullen:
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
        public IActionResult Bewerk(Analyse analyse, UitzendKrachtBesparingIndexViewModel model)
        {
            UitzendKrachtBesparing baat = KostOfBaatExtensions.GetBy(analyse.UitzendKrachtBesparingen, model.Id);

            if (ModelState.IsValid && baat != null)
            {
                // baat updaten
                baat.Id = model.Id;
                baat.Type = model.Type;
                baat.Soort = model.Soort;
                baat.Beschrijving = model.Beschrijving;
                baat.Bedrag = model.Bedrag;

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                TempData["message"] = "De baat is succesvol opgeslaan.";

                return RedirectToAction("Index", model);
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {
            UitzendKrachtBesparing baat = KostOfBaatExtensions.GetBy(analyse.UitzendKrachtBesparingen, id);

            analyse.UitzendKrachtBesparingen.Remove(baat);
            _analyseRepository.Save();

            UitzendKrachtBesparingIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De baat is succesvol verwijderd.";

            return View("Index", model);
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private UitzendKrachtBesparingIndexViewModel MaakModel(Analyse analyse)
        {
            UitzendKrachtBesparingIndexViewModel model = new UitzendKrachtBesparingIndexViewModel
            {
                Type = Type.Baat,
                Soort = Soort.UitzendkrachtBesparing,
                ViewModels = analyse
                                .UitzendKrachtBesparingen
                                .Select(m => new UitzendKrachtBesparingViewModel(m))
            };

            return model;
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.UitzendKrachtBesparingen.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            double totaal = analyse.UitzendKrachtBesparingen
                                    .Sum(t => t.Bedrag);

            ViewData["totaal"] = totaal.ToString("C");
        }
    }
}
