using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class ExtraOmzetController : Controller
    {

        private readonly IAnalyseRepository _analyseRepository;

        public ExtraOmzetController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            ExtraOmzetViewModel model = MaakModel(analyse);

            return View(model);
        }

        public IActionResult Opslaan(Analyse analyse, ExtraOmzetViewModel model)
        {
            if (ModelState.IsValid)
            {
                ExtraOmzet baat = new ExtraOmzet
                {
                    Type = model.Type,
                    Soort = model.Soort,
                    JaarbedragOmzetverlies = model.JaarbedragOmzetverlies,
                    Besparing = model.Besparing
                };

                analyse.ExtraOmzet = baat;
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De waarden zijn succesvol opgeslagen.";
            }

            return View("Index", model);
        }

        private ExtraOmzetViewModel MaakModel(Analyse analyse)
        {
            if (analyse.ExtraOmzet == null)
            {
                analyse.ExtraOmzet = new ExtraOmzet();
            }

            return new ExtraOmzetViewModel(analyse.ExtraOmzet);
        }
    }
}
