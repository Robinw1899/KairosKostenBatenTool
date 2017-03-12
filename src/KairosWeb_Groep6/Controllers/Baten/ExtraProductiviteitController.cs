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
    public class ExtraProductiviteitController : Controller
    {

        private readonly IAnalyseRepository _analyseRepository;

        public ExtraProductiviteitController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult VoegToe(Analyse analyse, ExtraProductiviteitIndexViewModel model /*UitzendKrachtBesparingIndexViewModel model*/)
        {
            if (ModelState.IsValid)
            {
                // de baat bestaat reeds:
                ExtraProductiviteit baat = new ExtraProductiviteit
                {
                    //Id = model.Id,
                    Id = 1,
                    Type = model.Type,
                    Soort = model.Soort,
                    Beschrijving = model.Beschrijving,
                    Bedrag = model.Bedrag
                };

                analyse.ExtraProductiviteit.Add(baat);
                _analyseRepository.Save();

                /*model = MaakModel(analyse);
                PlaatsTotaalInViewData(analyse);*/

                return PartialView("_OverzichtTabel", model.ViewModels);
            }

            /* PlaatsTotaalInViewData(analyse);*/

            return RedirectToAction("Index", model);
        }

        private ExtraProductiviteitIndexViewModel MaakModel(Analyse analyse)
        {
            ExtraProductiviteitIndexViewModel model = new ExtraProductiviteitIndexViewModel
            {
                Type = Models.Domain.Type.Baat,
                Soort = Soort.MedewerkersHogerNiveau,
                ViewModels = analyse
                                .MedewerkersHogerNiveauBaat
                                .Select(m => new ExtraProductiviteitViewModel(m))
            };

            return model;
        }
        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
