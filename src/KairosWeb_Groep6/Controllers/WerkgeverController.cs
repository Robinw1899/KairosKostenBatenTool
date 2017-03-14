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
        private readonly IWerkgeverRepository _werkgeverRepository;
        #endregion

        #region Constructors

        public WerkgeverController(
            IAnalyseRepository analyseRepository,
            IWerkgeverRepository werkgeverRepository)
        {
            _analyseRepository = analyseRepository;
            _werkgeverRepository = werkgeverRepository;
        }
        #endregion

        public IActionResult Index(Analyse analyse)
        {
            if (analyse.Werkgever == null)
            {
                // er is nog geen werkgever, doorsturen naar nieuwe analyse
                return RedirectToAction("NieuweAnalyse", "Analyse");
            }

            WerkgeverViewModel model = new WerkgeverViewModel(analyse.Werkgever);

            return View(model);
        }

        public IActionResult Opslaan(Analyse analyse, WerkgeverViewModel model)
        {
            Werkgever werkgever = _werkgeverRepository.GetById(model.WerkgeverId);

            werkgever.Naam = model.Naam;

            if (model.Straat != null && model.Nummer > 0)
            {
                werkgever.Straat = model.Straat;
                werkgever.Nummer = model.Nummer;
            }

            werkgever.Postcode = model.Postcode;
            werkgever.Gemeente = model.Gemeente;

            analyse.Werkgever = werkgever;

            _werkgeverRepository.Save();
            _analyseRepository.Save();

            return RedirectToAction("Index");
        }
    }
}
