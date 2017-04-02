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
        #endregion

        #region Constructors

        public WerkgeverController(
            IAnalyseRepository analyseRepository,
            IDepartementRepository werkgeverRepository)
        {
            _analyseRepository = analyseRepository;
            _departementRepository = werkgeverRepository;
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
            //analyse.Departement = new Departement();
            //analyse.Departement.Werkgever = new Werkgever();

            // model aanmaken
            WerkgeverViewModel model = new WerkgeverViewModel();

            _analyseRepository.Save();

            // view returnen
            return View(model);
        }

        [HttpPost]
        public IActionResult NieuweWerkgever(Analyse analyse, WerkgeverViewModel model)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            Departement departement = new Departement(model.Departement); // nieuw departement aanmaken
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
            //analyse = _analyseRepository.GetById(analyse.AnalyseId);

            Departement departement = _departementRepository.GetById(id);
            analyse.Departement = departement;

            _analyseRepository.Save();

            return RedirectToAction("Index", "Resultaat");
        }
    }
}
