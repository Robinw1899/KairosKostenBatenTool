using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(AnalyseFilter))]
    public class AnalyseController : Controller
    {
        #region Properties

        private readonly IAnalyseRepository _analyseRepository;
        private readonly IWerkgeverRepository _werkgeverRepository;
        #endregion

        #region Constructors

        public AnalyseController(
            IAnalyseRepository analyseRepository,
            IWerkgeverRepository werkgeverRepository)
        {
            _analyseRepository = analyseRepository;
            _werkgeverRepository = werkgeverRepository;
        }
        #endregion

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Baten");
        }

        public IActionResult NieuweAnalyse()
        {// hier word gekozen tussen een nieuwe of bestaande werkgever
            return View();
        }

        public IActionResult NieuweWerkgever(Analyse analyse)
        {
            // nieuwe werkgever aanmaken voor de analyse
            analyse.Werkgever = new Werkgever();

            // model aanmaken
            WerkgeverViewModel model = new WerkgeverViewModel(analyse.Werkgever);

            // view returnen
            return View(model);
        }

        [HttpPost]
        public IActionResult NieuweWerkgever(Analyse analyse, WerkgeverViewModel model)
        {
            Werkgever werkgever = analyse.Werkgever;
            werkgever.Naam = model.Naam;

            if (model.Straat != null && model.Nummer != 0)
            {
                werkgever.Straat = model.Straat;
                werkgever.Nummer = model.Nummer;
            }

            werkgever.Postcode = model.Postcode;
            werkgever.Gemeente = model.Gemeente;

            _werkgeverRepository.Add(werkgever);
            _werkgeverRepository.Save();

            _analyseRepository.Save();

            return View();
        }

        public IActionResult BestaandeWerkgever()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BestaandeWerkgever(string naam)
        {
            IEnumerable<Werkgever> werkgevers;

            if (naam.Equals(""))
                werkgevers = _werkgeverRepository.GetAll();
            else
            {
                werkgevers = _werkgeverRepository.GetByName(naam);
            }

            List<WerkgeverViewModel> viewModels = werkgevers.Select(w => new WerkgeverViewModel(w))
                .ToList();

            return PartialView("_Werkgevers", viewModels);
        }

        public IActionResult SelecteerBestaandeWerkgever(Analyse analyse, int id)
        {// nog verder veranderen naar redirect naar ResultaatController

            Werkgever werkgever = _werkgeverRepository.GetById(id);
            analyse.Werkgever = werkgever;

            _analyseRepository.Save();

            return RedirectToAction("Index", "Baten");
        }
    }
}
