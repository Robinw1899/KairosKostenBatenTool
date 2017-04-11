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
                // er is nog geen werkgever, doorsturen naar nieuwe analyse
                return RedirectToAction("SelecteerWerkgever");
            }

            WerkgeverViewModel model = new WerkgeverViewModel(analyse.Departement);

            return View(model);
        }

        public IActionResult Opslaan(Analyse analyse, WerkgeverViewModel model)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);
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
            return RedirectToAction("NieuweAnalyse", "Analyse");
        }

        public IActionResult NieuweWerkgever(Analyse analyse)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            // nieuwe werkgever aanmaken voor de analyse
            analyse.Departement = new Departement();
            analyse.Departement.Werkgever = new Werkgever();

            // model aanmaken
            WerkgeverViewModel model = new WerkgeverViewModel{PatronaleBijdrage = 35};

            _analyseRepository.Save();

            // view returnen
            return View(model);
        }

        [HttpPost]
        public IActionResult NieuweWerkgever(Analyse analyse, WerkgeverViewModel model)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);
            Departement departement = _departementRepository.GetDepByName(model.Departement);
            if (_departementRepository.GetByName(model.Naam) != null && departement != null && departement.Werkgever.Gemeente == model.Gemeente)
            {
                TempData["Error"] = "De Werkgever " + model.Naam +  "met als departement" + model.Departement +" bestaat al.";
                return RedirectToAction("NieuweWerkgever");
            }
           
           departement = new Departement(model.Departement);

            Werkgever werkgever = new Werkgever(); // nieuwe werkgever aanmaken

            werkgever.Naam = model.Naam;

            if (model.Straat != null && model.Nummer != 0)
            {
                werkgever.Straat = model.Straat;
                werkgever.Nummer = model.Nummer;
                werkgever.Bus = model.Bus;
            }

            werkgever.Postcode = model.Postcode;
            werkgever.Gemeente = model.Gemeente;
            werkgever.AantalWerkuren = model.AantalWerkuren;
            werkgever.PatronaleBijdrage = model.PatronaleBijdrage;

            departement.Werkgever = werkgever;

            // alles instellen
            _departementRepository.Add(departement);
            analyse.Departement = departement;

            // alles opslaan
            _departementRepository.Save();
            _analyseRepository.Save();

            TempData["message"] = "De werkgever is succesvol toegevoegd";

            return RedirectToAction("Index", "Resultaat");
        }

        public IActionResult AnnuleerNieuweWerkgever(Analyse analyse)
        {
            analyse.Departement = null;
            _analyseRepository.Save();

            return RedirectToAction("NieuweAnalyse", "Analyse");
        }

        public IActionResult BestaandeWerkgever()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BestaandeWerkgever(string naam)
        {
            IEnumerable<Departement> werkgevers;

            if (naam == null || naam.Equals(""))
                werkgevers = _departementRepository.GetAll();
            else
            {
                werkgevers = _departementRepository.GetByName(naam);
            }

            List<WerkgeverViewModel> viewModels = werkgevers.Select(w => new WerkgeverViewModel(w))
                .ToList();

            return PartialView("_Werkgevers", viewModels);
        }

        public IActionResult SelecteerBestaandeWerkgever(Analyse analyse, int id)
        {// nog verder veranderen naar redirect naar ResultaatController
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            //de werkgever is geselecteerd je moet dus naar overzicht departementen gaan voor specifieke werkgever
            Departement departement = _departementRepository.GetById(id);
            analyse.Departement = departement;

            _analyseRepository.Save();

            return RedirectToAction("Index", "Resultaat");
        }

        public IActionResult OverzichtDepartementenWerkgever(int id)
        {
            Departement departement = _departementRepository.GetById(id);
            WerkgeverViewModel model = new WerkgeverViewModel(departement);
            return View(model);
        }

        [HttpPost]
        public IActionResult OverzichtDepartementenWerkgever(int id,string naam)
        {

            IEnumerable<Departement> departementen;
            departementen = _departementRepository.GetListDepById(id);
            if (!(naam == null || naam.Equals("")))
                departementen = departementen.Where(t=>t.Naam.Contains(naam));
          

            return PartialView("_departementen", departementen);
        }

        public IActionResult NieuwDepartement(int id)
        {
            Departement departement = _departementRepository.GetById(id);
            WerkgeverViewModel model = new WerkgeverViewModel(departement);
            //het geselecteerd departement niet laten tonen
            return View(model);
        }

        [HttpPost]
        [ActionName("NieuwDepartement")]
        public IActionResult NieuwDepartementPost(Analyse analyse,WerkgeverViewModel model)
        {
            if (_departementRepository.GetDepByName(model.Departement) != null && _departementRepository.GetByName(model.Naam) != null)
            {
                TempData["Error"] = "Het departement " + model.Departement + "van de werkgever " + model.Naam + " bestaat al";
                return RedirectToAction("NieuwDepartement");
            } else
            {
                analyse = _analyseRepository.GetById(analyse.AnalyseId);
                TempData["Error"] = "";
                Departement departement = new Departement(model.Departement);

                Werkgever werkgever = _departementRepository.GetByName(model.Naam).First().Werkgever; // Zelfde werkgever maken

                werkgever.Naam = model.Naam;

                if (model.Straat != null && model.Nummer != 0)
                {
                    werkgever.Straat = model.Straat;
                    werkgever.Nummer = model.Nummer;
                    werkgever.Bus = model.Bus;
                }

                werkgever.Postcode = model.Postcode;
                werkgever.Gemeente = model.Gemeente;

                departement.Werkgever = werkgever;

                // alles instellen
                _departementRepository.Add(departement);
                analyse.Departement = departement;

                // alles opslaan
                _departementRepository.Save();
                _analyseRepository.Save();


                return RedirectToAction("Index", "Resultaat");
            }
          
        }

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

           return RedirectToAction("Index");
        }
    }
}
