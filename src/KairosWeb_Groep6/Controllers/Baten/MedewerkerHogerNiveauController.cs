using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten.MedewerkerNiveauBaatViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class MedewerkerHogerNiveauController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public MedewerkerHogerNiveauController(IAnalyseRepository analyseRepository)
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

                analyse.MedewerkersHogerNiveauBaat.Add(baat);
                _analyseRepository.Save();

                model = MaakModel(analyse);

                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            return RedirectToAction("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            MedewerkerNiveauBaat baat = analyse.MedewerkersHogerNiveauBaat
                                                .SingleOrDefault(b => b.Id == id);

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
            if (ModelState.IsValid)
            {
                MedewerkerNiveauBaat baat = analyse.MedewerkersHogerNiveauBaat
                                                 .SingleOrDefault(b => b.Id == model.Id);

                // parameters voor formulier instellen
                baat.Id = model.Id;
                baat.Type = model.Type;
                baat.Soort = model.Soort;
                baat.Uren = model.Uren;
                baat.BrutoMaandloonFulltime = model.BrutoMaandloonFulltime;
                _analyseRepository.Save();

                model = MaakModel(analyse);

                return RedirectToAction("Index", model);
            }

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            MedewerkerNiveauBaat baat = analyse.MedewerkersHogerNiveauBaat
                                                 .SingleOrDefault(b => b.Id == id);

            analyse.MedewerkersHogerNiveauBaat.Remove(baat);
            _analyseRepository.Save();

            IndexViewModel model = MaakModel(analyse);

            return View("Index", model);
        }

        private IndexViewModel MaakModel(Analyse analyse)
        {
            IndexViewModel model = new IndexViewModel
            {
                Type = Models.Domain.Type.Baat,
                Soort = Soort.MedewerkersHogerNiveau,
                ViewModels = analyse
                                .MedewerkersHogerNiveauBaat
                                .Select(m => new MedewerkerNiveauBaatViewModel(m))
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
