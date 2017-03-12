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
    public class ExtraOmzetController : Controller
    {

        private readonly IAnalyseRepository _analyseRepository;

        public ExtraOmzetController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult VoegToe(Analyse analyse, ExtraOmzetIndexViewModel model /*UitzendKrachtBesparingIndexViewModel model*/)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                ExtraOmzet baat = new ExtraOmzet
                {
                    //Id = model.Id,
                    Id = 1,
                    Type = model.Type,
                    Soort = model.Soort,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.ExtraOmzet.Add(baat);
                _analyseRepository.Save();

                /*model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);*/

                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            /* PlaatsTotaalInViewData(analyse);*/

            return RedirectToAction("Index", model);
        }
        private ExtraOmzetIndexViewModel MaakModel(Analyse analyse)
        {
            ExtraOmzetIndexViewModel model = new ExtraOmzetIndexViewModel
            {
                Type = Models.Domain.Type.Baat,
                Soort = Soort.MedewerkersHogerNiveau,
                ViewModels = analyse
                                .MedewerkersHogerNiveauBaat
                                .Select(m => new ExtraOmzetViewModel(m))
            };

            return model;
        }
        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
