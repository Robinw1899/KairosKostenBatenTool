using System;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.KairosViewModels.Baten.MedewerkerNiveauBaatViewModels;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class MedewerkersZelfdeNiveauController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public MedewerkersZelfdeNiveauController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            MedewerkerNiveauIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            PlaatsTotaalInViewData(analyse);
            return View(model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, MedewerkerNiveauIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                MedewerkerNiveauBaat baat = new MedewerkerNiveauBaat
                {
                    Type = model.Type,
                    Soort = model.Soort,
                    Uren = model.Uren,
                    BrutoMaandloonFulltime = model.BrutoMaandloonFulltime
                };

                analyse.MedewerkersZelfdeNiveauBaat.Add(baat);
                analyse.DatumLaatsteAanpassing = DateTime.Now;
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De baat is succesvol toegevoegd.";
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            MedewerkerNiveauBaat baat = KostOfBaatExtensions.GetBy(analyse.MedewerkersZelfdeNiveauBaat, id);

            MedewerkerNiveauIndexViewModel model = MaakModel(analyse);

            if (baat != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                model.Type = baat.Type;
                model.Soort = baat.Soort;
                model.Uren = baat.Uren;
                model.BrutoMaandloonFulltime = baat.BrutoMaandloonFulltime;
                model.ToonFormulier = 1;
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, MedewerkerNiveauIndexViewModel model)
        {// id is het id van de baat die moet bewerkt worden
            MedewerkerNiveauBaat baat = KostOfBaatExtensions.GetBy(analyse.MedewerkersZelfdeNiveauBaat, model.Id);

            if (ModelState.IsValid && baat != null)
            {
                // parameters voor formulier instellen
                baat.Id = model.Id;
                baat.Type = model.Type;
                baat.Soort = model.Soort;
                baat.Uren = model.Uren;
                baat.BrutoMaandloonFulltime = model.BrutoMaandloonFulltime;

                analyse.DatumLaatsteAanpassing = DateTime.Now;
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De baat is succesvol opgeslaan.";
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            MedewerkerNiveauBaat baat = KostOfBaatExtensions.GetBy(analyse.MedewerkersZelfdeNiveauBaat, id);

            if (baat != null)
            {
                analyse.MedewerkersZelfdeNiveauBaat.Remove(baat);
                analyse.DatumLaatsteAanpassing = DateTime.Now;
                _analyseRepository.Save();
            }

            MedewerkerNiveauIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De baat is succesvol verwijderd.";

            return View("Index", model);
        }

        private MedewerkerNiveauIndexViewModel MaakModel(Analyse analyse)
        {
            MedewerkerNiveauIndexViewModel model = new MedewerkerNiveauIndexViewModel
            {
                Type = Type.Baat,
                Soort = Soort.MedewerkersZelfdeNiveau,
                ViewModels = analyse
                                .MedewerkersZelfdeNiveauBaat
                                .Select(m => new MedewerkerNiveauBaatViewModel(m)
                    {
                        Bedrag = analyse.Departement == null
                        ? 0 : m.BerekenTotaleLoonkostPerJaar(analyse.Departement.Werkgever.AantalWerkuren, 
                                                                analyse.Departement.Werkgever.PatronaleBijdrage)
                    })
            };

            return model;
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        private void PlaatsTotaalInViewData(Analyse analyse)
        {
            if (analyse.MedewerkersZelfdeNiveauBaat.Count == 0)
            {
                ViewData["totaal"] = 0;
            }

            if(analyse.Departement != null) { 
                double totaal = MedewerkerNiveauBaatExtensions.GeefTotaal(
                    analyse.MedewerkersZelfdeNiveauBaat,
                    analyse.Departement.Werkgever.AantalWerkuren,
                    analyse.Departement.Werkgever.PatronaleBijdrage);

                ViewData["totaal"] = totaal.ToString("C");
            }
            else
            {
                ViewData["totaal"] = 0;
                TempData["error"] = "Opgelet! U heeft nog geen werkgever geselecteerd. Er zal dus nog geen resultaat " +
                                    "berekend worden bij deze kost.";
            }
        }
    }
}