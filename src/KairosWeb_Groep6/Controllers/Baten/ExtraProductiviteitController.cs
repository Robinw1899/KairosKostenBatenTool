using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class ExtraProductiviteitController : Controller
    {

        private readonly IAnalyseRepository _analyseRepository;

        public ExtraProductiviteitController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }

        public IActionResult Index(Analyse analyse)
        {
            var model = MaakModel(analyse);

            return View(model);
        }
        public IActionResult Opslaan(Analyse analyse, ExtraProductiviteitViewModel model)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                ExtraProductiviteit baat = new ExtraProductiviteit
                {
                    Type = model.Type,
                    Soort = model.Soort,
                    Bedrag = model.Bedrag
                };

                analyse.ExtraProductiviteit = baat;
                _analyseRepository.Save();

                model = MaakModel(analyse);

                TempData["message"] = "De baat is succesvol opgeslaan.";
            }

            return RedirectToAction("Index", model);
        }

        private ExtraProductiviteitViewModel MaakModel(Analyse analyse)
        {
            if (analyse.ExtraProductiviteit == null)
            {
                analyse.ExtraProductiviteit = new ExtraProductiviteit();
            }
           
            return new ExtraProductiviteitViewModel(analyse.ExtraProductiviteit);
        }
    }
}
