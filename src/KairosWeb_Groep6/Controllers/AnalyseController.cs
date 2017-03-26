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
    [ServiceFilter(typeof(JobcoachFilter))]
    public class AnalyseController : Controller
    {
        #region Properties

        private readonly IAnalyseRepository _analyseRepository;
        private readonly IDepartementRepository _departementRepository;
        private readonly IJobcoachRepository _jobcoachRepository;
        #endregion

        #region Constructors

        public AnalyseController(
            IAnalyseRepository analyseRepository,
            IDepartementRepository werkgeverRepository,
            IJobcoachRepository jobcoachRepository)
        {
            _analyseRepository = analyseRepository;
            _departementRepository = werkgeverRepository;
            _jobcoachRepository = jobcoachRepository;
        }
        #endregion

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Baten");
        }

        public IActionResult NieuweAnalyse(Analyse analyse, Jobcoach jobcoach)
        {//hier word gekozen tussen een nieuwe of bestaande werkgever
            if (analyse.AnalyseId == 0)
            {
                if (jobcoach != null)
                {
                    jobcoach = _jobcoachRepository.GetById(jobcoach.JobcoachId);
                    jobcoach.Analyses.Add(analyse);
                    _jobcoachRepository.Save();
                }

                _analyseRepository.Add(analyse);
                _analyseRepository.Save();
            }
            
            return View();
        }

        public IActionResult SelecteerWerkgever()
        {
            return View("NieuweAnalyse");
        }

        public IActionResult NieuweWerkgever(Analyse analyse)
        {
            analyse = _analyseRepository.GetById(analyse.AnalyseId);

            // nieuwe werkgever aanmaken voor de analyse
            analyse.Departement = new Departement();
            analyse.Departement.Werkgever = new Werkgever();

            // model aanmaken
            WerkgeverViewModel model = new WerkgeverViewModel(analyse.Departement);

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
