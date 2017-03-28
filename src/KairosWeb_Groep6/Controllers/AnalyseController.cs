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
    public class AnalyseController : Controller
    {
        #region Properties
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IJobcoachRepository _jobcoachRepository;
        #endregion

        #region Constructors

        public AnalyseController(
            IAnalyseRepository analyseRepository,
            IJobcoachRepository jobcoachRepository)
        {
            _analyseRepository = analyseRepository;
            _jobcoachRepository = jobcoachRepository;
        }
        #endregion

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Baten");
        }

        [ServiceFilter(typeof(AnalyseFilter))]
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult NieuweAnalyse(Analyse analyse, Jobcoach jobcoach)
        {//hier word gekozen tussen een nieuwe of bestaande werkgever
            if (analyse.AnalyseId == 0)
            {
                _analyseRepository.Add(analyse);
                _analyseRepository.Save();

                if (jobcoach != null)
                {
                    jobcoach = _jobcoachRepository.GetById(jobcoach.JobcoachId);
                    jobcoach.Analyses.Add(analyse);
                    _jobcoachRepository.Save();
                }
            }
            
            return View();
        }

        public IActionResult OpenAnalyse(int id)
        {
            Analyse analyse = _analyseRepository.GetById(id); // analyse instellen in Session
            AnalyseFilter.SetAnalyseInSession(HttpContext, analyse);

            return RedirectToAction("Index", "Resultaat");
        }

        public IActionResult VerwijderAnalyse(int id)
        {
            try
            {
                Analyse analyse = _analyseRepository.GetById(id);
                _analyseRepository.Remove(analyse);
                _analyseRepository.Save();

                if (analyse.Departement == null)
                {
                    TempData["message"] = "De analyse is succesvol verwijderd.";
                }
                else
                {
                    TempData["message"] = $"De analyse van {analyse.Departement.Werkgever.Naam} - {analyse.Departement.Naam}" +
                                          " is succesvol verwijderd.";
                }
            }
            catch
            {
                TempData["error"] = "Er ging onverwachts iets fout, probeer later opnieuw.";
            }
            

            return RedirectToAction("Index", "Kairos");
        }
    }
}
