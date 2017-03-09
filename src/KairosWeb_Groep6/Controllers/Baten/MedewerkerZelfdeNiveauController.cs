using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten.MedewerkerNiveauBaatViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class MedewerkerZelfdeNiveauController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public MedewerkerZelfdeNiveauController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            IndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                MedewerkerNiveauBaat baat = new MedewerkerNiveauBaat
                {
                    //Id = model.Id,
                    Id = 1,
                    Type = model.Type,
                    Soort = model.Soort,
                    Uren = model.Uren,
                    BrutoMaandloonFulltime = model.BrutoMaandloonFulltime
                };

                analyse.MedewerkersZelfdeNiveauBaat.Add(baat);
                _analyseRepository.Save();

                model = MaakModel(analyse);

                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            return RedirectToAction("Index", model);
        }

        private IndexViewModel MaakModel(Analyse analyse)
        {
            IndexViewModel model = new IndexViewModel
            {
                Type = Type.Baat,
                Soort = Soort.MedewerkersZelfdeNiveau,
                ViewModels = analyse
                                .MedewerkersZelfdeNiveauBaat
                                .Select(m => new MedewerkerNiveauBaatViewModel(m))
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt worden
            MedewerkerNiveauBaat baat = analyse.MedewerkersZelfdeNiveauBaat.GetBy(id);

            IndexViewModel model = MaakModel(analyse);

            // parameters voor formulier instellen
            model.Id = id;
            model.Type = baat.Type;
            model.Soort = baat.Soort;
            model.Uren = baat.Uren;
            model.BrutoMaandloonFulltime = baat.BrutoMaandloonFulltime;

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, IndexViewModel model)
        {// id is het id van de baat die moet bewerkt worden
            MedewerkerNiveauBaat baat = analyse.MedewerkersZelfdeNiveauBaat.GetBy(model.Id);

            // parameters voor formulier instellen
            baat.Id = model.Id;
            baat.Type = model.Type;
            baat.Soort = model.Soort;
            baat.Uren = model.Uren;
            baat.BrutoMaandloonFulltime = model.BrutoMaandloonFulltime;

            model = MaakModel(analyse);

            return RedirectToAction("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            MedewerkerNiveauBaat baat = (MedewerkerNiveauBaat)analyse.MedewerkersZelfdeNiveauBaat.GetBy(id);
            analyse.MedewerkersZelfdeNiveauBaat.Remove(baat);

            IndexViewModel model = MaakModel(analyse);

            return View("Index", model);
        }
    }
}