using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    public class WerkgeverController : Controller
    {
        #region Properties

        private readonly IAnalyseRepository _analyseRepository;
        private readonly IDepartementRepository _departementRepository;
        private readonly IWerkgeverRepository _werkgeverRepository;
        #endregion

        #region Constructors

        public WerkgeverController(
            IAnalyseRepository analyseRepository,
            IDepartementRepository departementenRepository,
            IWerkgeverRepository werkgeverRepository)
        {
            _analyseRepository = analyseRepository;
            _departementRepository = departementenRepository;
            _werkgeverRepository = werkgeverRepository;
        }
        #endregion

        public IActionResult Index(Analyse analyse)
        {
            if (analyse.Departement == null || analyse.Departement.Naam.Length == 0)
            {
                // er is nog geen werkgever, vragen om een werkgever te selecteren
                return RedirectToAction("SelecteerWerkgever");
            }

            WerkgeverViewModel model = new WerkgeverViewModel(analyse.Departement);

            return View(model);
        }

        public IActionResult Opslaan(Analyse analyse, WerkgeverViewModel model)
        {
            Departement departement = _departementRepository.GetById(model.DepartementId);
            Werkgever werkgever = departement.Werkgever;

            werkgever.Naam = model.Naam;

            if (model.Straat != null && model.Nummer > 0)
            {
                werkgever.Straat = model.Straat;
                werkgever.Nummer = model.Nummer;
                werkgever.Bus = model.Bus;
            }

            werkgever.Postcode = model.Postcode;
            werkgever.Gemeente = model.Gemeente;

            departement.Naam = model.Naam;
            departement.Werkgever = werkgever;

            analyse.Departement = departement;

            _departementRepository.Save();
            _analyseRepository.Save();

            return RedirectToAction("Index");
        }

        public IActionResult SelecteerWerkgever()
        {
            return View("SelecteerWerkgever");
        }

        #region Nieuwe werkgever
        public IActionResult NieuweWerkgever(Analyse analyse)
        {
            // model aanmaken
            WerkgeverViewModel model = new WerkgeverViewModel{PatronaleBijdrage = 35};

            // view returnen
            return View(model);
        }

        [HttpPost]
        public IActionResult NieuweWerkgever(Analyse analyse, WerkgeverViewModel model)
        {
            Departement departement = _departementRepository.GetByName(model.Departement);

            // de werkgever al aanmaken, zodat straks de controle kan gebeuren
            Werkgever werkgever = new Werkgever
            {  // nieuwe werkgever aanmaken
                Naam = model.Naam,
                Postcode = model.Postcode,
                Gemeente = model.Gemeente,
                AantalWerkuren = model.AantalWerkuren,
                PatronaleBijdrage = model.PatronaleBijdrage
            };

            if (model.Straat != null && model.Nummer != 0)
            {
                // straat en nummer zijn niet verplicht,
                // maar als ze ingevuld zijn, instellen in de werkgever
                werkgever.Straat = model.Straat;
                werkgever.Nummer = model.Nummer;
                werkgever.Bus = model.Bus;
            }

            bool result = ControleerBestaandDepartement(departement, werkgever);

            if (result)
            {
                TempData["Error"] = "De werkgever " + model.Naam + " met als departement " + model.Departement + " bestaat al.";

                // terugsturen naar het formulier
                return RedirectToAction("NieuweWerkgever", model);
            }

            // anders maken we een nieuw departement aan
            departement = new Departement(model.Departement) {Werkgever = werkgever};

            // alles instellen
            _departementRepository.Add(departement);
            analyse.Departement = departement;

            // alles opslaan
            _departementRepository.Save();
            _analyseRepository.Save();

            TempData["message"] = "De werkgever is succesvol toegevoegd";

            return RedirectToAction("Index", "Resultaat");
        }
        #endregion

        #region Bestaande wergever
        public IActionResult BestaandeWerkgever()
        {
            IEnumerable<Werkgever> werkgevers = _werkgeverRepository.GetAll().Take(10).ToList();
            BestaandeWerkgeverViewModel model = new BestaandeWerkgeverViewModel
            {
                Werkgevers = werkgevers.Select(w => new WerkgeverViewModel(w))
                                        .ToList()
            };

            return View(model);
        }

        public IActionResult SelecteerBestaandeWerkgever(Analyse analyse, int id, int werkgeverid)
        { // id is het id van het departement dat geselecteerd werd
            Departement departement = _departementRepository.GetById(id);
            analyse.Departement = departement;

            _analyseRepository.Save();

            AnalyseFilter.SetAnalyseInSession(HttpContext, analyse);

            return RedirectToAction("Index", "Resultaat");
        }
        #endregion

        #region Zoekmethoden
        public IActionResult ZoekDepartementen(int id, string naam)
        { // id is id van werkgever
            IEnumerable<Departement> departementen = _departementRepository.GetAllVanWerkgever(id);

            if (naam != null)
            {
                departementen = departementen
                    .Where(d => d.Naam.IndexOf(naam, StringComparison.OrdinalIgnoreCase) >= 0); // departementen filteren
            }

            var models = departementen.Select(d => new DepartementViewModel(d)
            {
                WerkgeverId = id
            });

            return PartialView("_Departementen", models);
        }

        [HttpPost]
        public IActionResult ZoekWerkgever(string naam)
        {
            IEnumerable<Werkgever> werkgevers;

            if (naam == null || naam.Equals(""))
                werkgevers = _werkgeverRepository.GetAll();
            else
            {
                werkgevers = _werkgeverRepository.GetByName(naam);
            }

            List<WerkgeverViewModel> viewModels = werkgevers.Select(w => new WerkgeverViewModel(w))
                .ToList();

            return PartialView("_Werkgevers", viewModels);
        }
        #endregion

        #region Bestaand departement
        public IActionResult BestaandDepartement(int id)
        {// id is is het id van de werkgever
            IEnumerable<Departement> departementen = _departementRepository.GetAllVanWerkgever(id);

            BestaandDepartementViewModel model = new BestaandDepartementViewModel
            {
                WerkgeverId = id,
                Departementen = departementen
                    .Select(d => new DepartementViewModel(d)
                    {
                        WerkgeverId = id
                    })
                    .AsEnumerable()
            };

            return View(model);
        }
        #endregion

        #region Nieuw departement
        public IActionResult NieuwDepartement(int id)
        { 
            // id is het id van de werkgever
            // werkgever ophalen
            Werkgever werkgever = _werkgeverRepository.GetById(id);

            // viewmodel aanmaken
            WerkgeverViewModel model = new WerkgeverViewModel(werkgever);
            
            return View(model);
        }

        [HttpPost]
        public IActionResult NieuwDepartement(Analyse analyse, WerkgeverViewModel model)
        {
            Departement departement = _departementRepository.GetByName(model.Departement);
            Werkgever werkgever = new Werkgever
            {  // nieuwe werkgever aanmaken
                Naam = model.Naam,
                Postcode = model.Postcode,
                Gemeente = model.Gemeente,
                AantalWerkuren = model.AantalWerkuren,
                PatronaleBijdrage = model.PatronaleBijdrage
            };

            if (model.Straat != null && model.Nummer != 0)
            {
                // straat en nummer zijn niet verplicht,
                // maar als ze ingevuld zijn, instellen in de werkgever
                werkgever.Straat = model.Straat;
                werkgever.Nummer = model.Nummer;
                werkgever.Bus = model.Bus;
            }

            bool result = ControleerBestaandDepartement(departement, werkgever);

            if (result)
            {
                TempData["Error"] = "De werkgever " + model.Naam + " met als departement " + model.Departement + " bestaat al.";

                // terugsturen naar het formulier
                return RedirectToAction("NiewDepartement", model.WerkgeverId);
            }

            departement = new Departement(model.Departement);

            werkgever = _werkgeverRepository.GetById(model.WerkgeverId);

            departement.Werkgever = werkgever;

            // alles instellen
            _departementRepository.Add(departement);
            analyse.Departement = departement;

            // alles opslaan
            _departementRepository.Save();
            _analyseRepository.Save();


            return RedirectToAction("Index", "Resultaat");
        }
        #endregion

        #region Contactpersonen
        public IActionResult VoegContactPersoonToe(int id)
        {     
            ContactPersoonViewModel model = new ContactPersoonViewModel();
            model.WerkgeverId = id;
            return View(model);
        }
        [HttpPost]
        public IActionResult VoegContactPersoonToe(ContactPersoonViewModel cpViewModel)
        {
         
            Werkgever werkgever = _werkgeverRepository.GetById(cpViewModel.WerkgeverId);
            ContactPersoon cp = new ContactPersoon(cpViewModel.Voornaam, cpViewModel.Naam, cpViewModel.Email);
          
             werkgever.ContactPersonen.Add(cp);
            _werkgeverRepository.Save();
            _departementRepository.Save();

           return RedirectToAction("Index", "ContactPersoon");
        }
        #endregion

        #region Helpers
        private bool ControleerBestaandDepartement(Departement departement, Werkgever werkgever)
        {
            if (departement != null)
            {
                // het departement bestaat al, kijken of de werkgever ook al bestaat
                Werkgever other = departement.Werkgever;

                if (string.Equals(werkgever.Naam, other.Naam)
                    && string.Equals(werkgever.Straat, other.Straat)
                    && werkgever.Nummer == other.Nummer
                    && werkgever.Postcode == other.Postcode
                    && string.Equals(werkgever.Gemeente, other.Gemeente)
                    && werkgever.AantalWerkuren.Equals(other.AantalWerkuren))
                {
                    // beiden bestaan al
                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
