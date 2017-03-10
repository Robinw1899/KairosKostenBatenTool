using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten.UitzendKrachtBesparingViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class UitzendKrachtBesparingController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public UitzendKrachtBesparingController(IAnalyseRepository analyseRepository)
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

        public IActionResult VoegToe(Analyse analyse, IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                UitzendKrachtBesparing baat = new UitzendKrachtBesparing
                {
                    //Id = model.Id,
                    Id = 1,
                    Type = model.Type,
                    Soort = model.Soort,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.UitzendKrachtBesparingen.Add(baat);
                _analyseRepository.Save();

                model = MaakModel(analyse);

                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            return RedirectToAction("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {
            UitzendKrachtBesparing baat = analyse.UitzendKrachtBesparingen
                                                    .SingleOrDefault(u => u.Id == id);

            IndexViewModel model = MaakModel(analyse);

            // gegevens analyse die bewerkt wordt invullen:
            model.Id = id;
            model.Type = baat.Type;
            model.Soort = baat.Soort;
            model.Beschrijving = baat.Beschrijving;
            model.Bedrag = baat.Bedrag;

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, IndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                UitzendKrachtBesparing baat = analyse.UitzendKrachtBesparingen
                                                .SingleOrDefault(b => b.Id == model.Id);

                // baat updaten
                baat.Id = model.Id;
                baat.Type = model.Type;
                baat.Soort = model.Soort;
                baat.Beschrijving = model.Beschrijving;
                baat.Bedrag = model.Bedrag;

                model = MaakModel(analyse);

                return RedirectToAction("Index", model);
            }

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {
            UitzendKrachtBesparing baat = analyse.UitzendKrachtBesparingen
                                                    .SingleOrDefault(u => u.Id == id);

            analyse.UitzendKrachtBesparingen.Remove(baat);
            // hier moet nog extra komen wnr db werkt
            _analyseRepository.Save();

            IndexViewModel model = MaakModel(analyse);

            return View("Index", model);
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private IndexViewModel MaakModel(Analyse analyse)
        {
            IndexViewModel model = new IndexViewModel
            {
                Type = Type.Baat,
                Soort = Soort.MedewerkersZelfdeNiveau,
                ViewModels = analyse
                                .UitzendKrachtBesparingen
                                .Select(m => new UitzendKrachtBesparingViewModel(m))
            };

            return model;
        }
    }
}
