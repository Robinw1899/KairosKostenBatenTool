using System.Linq;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.LoonKostViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KairosWeb_Groep6.Controllers.Kosten
{
    public class LoonKostController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public LoonKostController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }
        // GET: /<controller>/
        public IActionResult Index(Analyse analyse)
        {
            LoonKostIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            PlaatsTotaalInViewData(analyse);

            return View(model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, LoonKostIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                Loonkost kost = new Loonkost
                {
                    //Id = model.Id,
                    Id = 1,
                    //functie moet nog toegevoegd worden
                    AantalUrenPerWeek = model.AantalUrenPerWeek,
                    BrutoMaandloonFulltime = model.BrutoMaandloonFulltime,
                    Doelgroep = model.Doelgroep,
                    Ondersteuningspremie = model.Ondersteuningspremie,
                    AantalMaandenIBO = model.AantalMaandenIBO,
                    Bedrag = model.Bedrag
                };

                analyse.Loonkosten.Add(kost);
                _analyseRepository.Save();

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);


                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            PlaatsTotaalInViewData(analyse);

            return RedirectToAction("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            Loonkost kost = analyse.Loonkosten
                                              .SingleOrDefault(b => b.Id == id);

            LoonKostIndexViewModel model = MaakModel(analyse);

            if (kost != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                //functie
                model.AantalUrenPerWeek = kost.AantalUrenPerWeek;
                model.BrutoMaandloonFulltime = kost.BrutoMaandloonFulltime;
                model.Doelgroep = kost.Doelgroep;
                model.Ondersteuningspremie = kost.Ondersteuningspremie;
                model.AantalMaandenIBO = kost.AantalMaandenIBO;
                model.Bedrag = kost.Bedrag;
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, LoonKostIndexViewModel model)
        {// id is het id van de baat die moet bewerkt worden
            Loonkost kost = analyse.Loonkosten
                                             .SingleOrDefault(b => b.Id == model.Id);

           
            if (ModelState.IsValid && kost != null)
            {
                // parameters voor formulier instellen
                model.Id = kost.Id;
                //functie
                model.AantalUrenPerWeek = kost.AantalUrenPerWeek;
                model.BrutoMaandloonFulltime = kost.BrutoMaandloonFulltime;
                model.Doelgroep = kost.Doelgroep;
                model.Ondersteuningspremie = kost.Ondersteuningspremie;
                model.AantalMaandenIBO = kost.AantalMaandenIBO;
                model.Bedrag = kost.Bedrag;

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                return RedirectToAction("Index", model);
            }
            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            Loonkost kost = analyse.Loonkosten
                                                 .SingleOrDefault(k => k.Id == id);
            if (kost != null)
            {
                analyse.Loonkosten.Remove(kost);
                _analyseRepository.Save();
            }

            LoonKostIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De waarden zijn succesvol verwijderd.";

            return View("Index", model);
        }
        private LoonKostIndexViewModel MaakModel(Analyse analyse)
        {
            LoonKostIndexViewModel model = new LoonKostIndexViewModel()
            {
                //Type = Type.Baat,
                //Soort = Soort.MedewerkersZelfdeNiveau,
                ViewModels = analyse
                                .Loonkosten
                                .Select(m => new LoonkostViewModel(m))
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.Loonkosten.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            double totaal = analyse.Loonkosten
                                    .Sum(t => t.Bedrag);

            ViewData["totaal"] = totaal.ToString("C");
        }
    }
}

