using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class SubsidieController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public SubsidieController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            SubsidieViewModel model = MaakModel(analyse);

            return View(model);
        }

        [HttpPost]
        public IActionResult Opslaan(Analyse analyse, SubsidieViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                Subsidie baat = new Subsidie
                {
                    Type = model.Type,
                    Soort = model.Soort,
                    Bedrag = model.Bedrag
                };

                analyse.Subsidie = baat;
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De baat is succesvol opgeslaan.";
            }

            return View("Index", model);
        }

        private SubsidieViewModel MaakModel(Analyse analyse)
        {
            if (analyse.Subsidie == null)
            {
                analyse.Subsidie = new Subsidie();
            }

            return new SubsidieViewModel(analyse.Subsidie);
        }
    }
}
