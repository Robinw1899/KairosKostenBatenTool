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
                return RedirectToAction("NieuweAnalyse", "Analyse");
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
    }
}
