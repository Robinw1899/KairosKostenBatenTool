using System.Linq;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten.ExtraBesparingViewModels;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class ExtraBesparingenController : Controller
    {

        private readonly IAnalyseRepository _analyseRepository;

        public ExtraBesparingenController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }
 
        public IActionResult Index(Analyse analyse)
        {
            ExtraBesparingIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            return View(model);
        }

        public IActionResult VoegToe(Analyse analyse, ExtraBesparingIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                ExtraBesparing baat = new ExtraBesparing()
                {
                    //Id = model.Id,
                    Id = 1,
                    Type = model.Type,
                    Soort = model.Soort,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.ExtraBesparingen.Add(baat);
                _analyseRepository.Save();

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            return RedirectToAction("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            ExtraBesparing baat = analyse.ExtraBesparingen
                                                .SingleOrDefault(b => b.Id == id);

            ExtraBesparingIndexViewModel model = MaakModel(analyse);

            if (baat != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                model.Type = baat.Type;
                model.Soort = baat.Soort;
                model.Beschrijving = baat.Beschrijving;
                model.Bedrag = baat.Bedrag;
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, ExtraBesparingIndexViewModel model)
        {// id is het id van de baat die moet bewerkt worden
            ExtraBesparing baat = analyse.ExtraBesparingen
                                                 .SingleOrDefault(b => b.Id == model.Id);

            if (ModelState.IsValid && baat != null)
            {
                // parameters voor formulier instellen
                baat.Id = model.Id;
                baat.Type = model.Type;
                baat.Soort = model.Soort;
                baat.Beschrijving = model.Beschrijving;
                baat.Bedrag = model.Bedrag;

                _analyseRepository.Save();

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                return RedirectToAction("Index", model);
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            ExtraBesparing baat = analyse.ExtraBesparingen
                                                 .SingleOrDefault(b => b.Id == id);
            if (baat != null)
            {
                analyse.ExtraBesparingen.Remove(baat);
                _analyseRepository.Save();
            }

            ExtraBesparingIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        private ExtraBesparingIndexViewModel MaakModel(Analyse analyse)
        {
            ExtraBesparingIndexViewModel model = new ExtraBesparingIndexViewModel
            {
                Type = Type.Baat,
                Soort = Soort.ExtraBesparing,
                ViewModels = analyse
                                .ExtraBesparingen
                                .Select(m => new ExtraBesparingViewModel(m))
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.ExtraBesparingen.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            double totaal = analyse.ExtraBesparingen
                                    .Sum(t => t.Bedrag);

            ViewData["totaal"] = totaal.ToString("C");
        }
    }
}
