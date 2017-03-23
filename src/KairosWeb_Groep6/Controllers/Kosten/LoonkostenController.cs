using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.LoonKostViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Controllers.Kosten
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    public class LoonkostenController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public LoonkostenController(IAnalyseRepository analyseRepository, IJobcoachRepository jobcoachRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse, bool foutgegeven = false)
        {
            LoonkostenIndexViewModel model = MaakModel(analyse);

            if (IsAjaxRequest())
            {
                PlaatsTotaalInViewData(analyse);
                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            PlaatsTotaalInViewData(analyse);

            return View(model);
        }

        [HttpPost]
        public IActionResult VoegToe(Analyse analyse, Jobcoach jobcoach, LoonkostenIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                Loonkost kost = new Loonkost
                {
                    Beschrijving = model.Beschrijving,
                    AantalUrenPerWeek = model.AantalUrenPerWeek,
                    BrutoMaandloonFulltime = model.BrutoMaandloonFulltime,
                    Doelgroep = model.Doelgroep,
                    Ondersteuningspremie = model.Ondersteuningspremie,
                    AantalMaandenIBO = model.AantalMaandenIBO,
                    IBOPremie = model.IBOPremie
                };

                analyse.Loonkosten.Add(kost);
                _analyseRepository.Save();

                model = MaakModel(analyse);
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Bewerk(Analyse analyse, int id)
        {// id is het id van de baat die moet bewerkt wordens
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            Loonkost kost = KostOfBaatExtensions.GetBy(analyse.Loonkosten, id);

            LoonkostenIndexViewModel model = MaakModel(analyse);

            if (kost != null)
            {
                // parameters voor formulier instellen
                model.Id = id;
                model.Beschrijving = kost.Beschrijving;
                model.AantalUrenPerWeek = kost.AantalUrenPerWeek;
                model.BrutoMaandloonFulltime = kost.BrutoMaandloonFulltime;
                model.Doelgroep = kost.Doelgroep;
                model.Ondersteuningspremie = kost.Ondersteuningspremie;
                model.AantalMaandenIBO = kost.AantalMaandenIBO;
                model.IBOPremie = kost.IBOPremie;
            }

            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        [HttpPost]
        public IActionResult Bewerk(Analyse analyse, LoonkostenIndexViewModel model)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            Loonkost kost = KostOfBaatExtensions.GetBy(analyse.Loonkosten, model.Id);


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

                model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);

                if (model.Doelgroep == null)
                {
                    TempData["error"] =
                        "Opgelet! U heeft nog geen doelgroep geselecteerd. Er zal dus nog geen resultaat " +
                        "berekend worden bij deze kost.";
                }

                if (analyse.Departement == null)
                {
                    // return de View zodat de error rond de werkgever toch getoond wordt
                    return View("Index", model);
                }

                return View("Index", model);
            }
            PlaatsTotaalInViewData(analyse);

            return View("Index", model);
        }

        public IActionResult Verwijder(Analyse analyse, int id)
        {// id is het id van de baat die moet verwijderd worden
            Loonkost kost = KostOfBaatExtensions.GetBy(analyse.Loonkosten, id);

            if (kost != null)
            {
                analyse.Loonkosten.Remove(kost);
                _analyseRepository.Save();
            }

            LoonkostenIndexViewModel model = MaakModel(analyse);
            PlaatsTotaalInViewData(analyse);

            TempData["message"] = "De waarden zijn succesvol verwijderd.";

            return View("Index", model);
        }
        private LoonkostenIndexViewModel MaakModel(Analyse analyse)
        {
            Array values = Enum.GetValues(typeof(Doelgroep));
            IList<Doelgroep> doelgroepen = new List<Doelgroep>();

            foreach (Doelgroep value in values)
            {
                doelgroepen.Add(value);
            }

            LoonkostenIndexViewModel model = new LoonkostenIndexViewModel(doelgroepen , Doelgroep.Andere)
            {
                Type = Type.Kost,
                Soort = Soort.Loonkost,
                ViewModels = analyse
                                .Loonkosten
                                .Select(m => new LoonkostViewModel(m)
                    {
                        Bedrag = analyse.Departement == null
                        ? 0 : 
                        m.BerekenTotaleLoonkost(analyse.Departement.Werkgever.AantalWerkuren, analyse.Departement.Werkgever.PatronaleBijdrage)
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
            if (analyse.Loonkosten.Count == 0)
            {
                ViewData["totaalBrutolonen"] = 0;
                ViewData["totaalLoonkosten"] = 0;
            }

            if (analyse.Departement != null)
            {
                double totaal = LoonkostExtensions.GeefTotaalBrutolonenPerJaarAlleLoonkosten(
                    analyse.Loonkosten, analyse.Departement.Werkgever.AantalWerkuren,
                    analyse.Departement.Werkgever.PatronaleBijdrage);

                ViewData["totaalBrutolonen"] = totaal.ToString("C");

                totaal = LoonkostExtensions.GeefTotaalAlleLoonkosten(
                    analyse.Loonkosten, analyse.Departement.Werkgever.AantalWerkuren,
                    analyse.Departement.Werkgever.PatronaleBijdrage);

                ViewData["totaalLoonkosten"] = totaal.ToString("C");
            }
            else
            {
                ViewData["totaalBrutolonen"] = 0;
                ViewData["totaalLoonkosten"] = 0;
                TempData["error"] = "Opgelet! U heeft nog geen werkgever geselecteerd. Er zal dus nog geen resultaat " +
                                    "berekend worden bij deze kost.";
            }
        }
    }
}

