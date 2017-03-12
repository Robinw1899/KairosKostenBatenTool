using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KairosWeb_Groep6.Controllers.Baten
{
    [ServiceFilter(typeof(AnalyseFilter))]
    public class ExterneInkoopController : Controller
    {
        private readonly IAnalyseRepository _analyseRepository;

        public ExterneInkoopController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }
        // GET: /<controller>/
        public IActionResult Index(Analyse analyse)
        {
            ExterneInkoopViewModel model = new ExterneInkoopViewModel();
            if (IsAjaxRequest())
            {

            }
            return View();
        }
        public IActionResult VoegToe(Analyse analyse, ExterneInkoopViewModel model /*UitzendKrachtBesparingIndexViewModel model*/)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                ExterneInkoop baat = new ExterneInkoop
                {
                    //Id = model.Id,
                    Id = 1,
                    Type = model.Type,
                    Soort = model.Soort,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.ExterneInkopen.Add(baat);
                _analyseRepository.Save();

                /*model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);*/

                return PartialView("_OverzichtTabel", model.ViewModels);
            }

           /* PlaatsTotaalInViewData(analyse);*/

            return RedirectToAction("Index", model);
        }
        private ExterneInkoopIndexViewModel MaakModel(Analyse analyse)
        {
            ExterneInkoopIndexViewModel model = new ExterneInkoopIndexViewModel
            {
                Type = Models.Domain.Type.Baat,
                Soort = Soort.MedewerkersHogerNiveau,
                ViewModels = analyse
                                .MedewerkersHogerNiveauBaat
                                .Select(m => new ExterneInkoopViewModel(m))
            };

            return model;
        }
        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
