using System;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
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

        #region NieuweAnalyse
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult NieuweAnalyse(Jobcoach jobcoach)
        {
            try
            {
                Analyse analyse = new Analyse();

                _analyseRepository.Add(analyse);
                _analyseRepository.Save();

                if (jobcoach != null)
                {
                    jobcoach = _jobcoachRepository.GetById(jobcoach.PersoonId);
                    jobcoach.Analyses.Add(analyse);
                    _jobcoachRepository.Save();

                    if (HttpContext != null)
                    {// nodig voor testen, HttpContext kan je niet mocken
                        AnalyseFilter.SetAnalyseInSession(HttpContext, analyse);
                    }

                    return RedirectToAction("SelecteerWerkgever", "Werkgever");
                }
            }
            catch
            {
                TempData["error"] = "Er ging iets fout tijdens het starten van een nieuwe analyse, probeer later opnieuw";
            }
            
            return RedirectToAction("Index", "Kairos");
        }
        #endregion

        #region OpenAnalyse
        public IActionResult OpenAnalyse(int id)
        {
            try
            {
                Analyse analyse = _analyseRepository.GetById(id);

                if (HttpContext != null)
                {// nodig voor testen, HttpContext kan je niet mocken
                    // analyse instellen in Session
                    AnalyseFilter.SetAnalyseInSession(HttpContext, analyse);
                }

                return RedirectToAction("Index", "Resultaat");
            }
            catch
            {
                TempData["error"] = "Er ging iets fout tijdens het ophalen van de analyse, probeer later opnieuw";
            }

            return RedirectToAction("Index", "Kairos");
        }
        #endregion

        #region VerwijderAnalyse
        public IActionResult VerwijderAnalyse(int id, string from)
        {
            try
            {
                Analyse analyse = _analyseRepository.GetById(id);

                ViewData["analyseId"] = id;

                if (analyse.Departement != null)
                {
                    ViewData["werkgever"] = $"{analyse.Departement.Werkgever.Naam} - {analyse.Departement.Naam}";
                }

                ViewData["returnUrl"] = from;

                return View();
            }
            catch
            {
                TempData["error"] =
                    "Er ging iets fout tijdens het voorbereiden van het verwijderen, probeer later opnieuw";
            }

            return RedirectToAction("Index", "Kairos");
        }

        [HttpPost]
        [ActionName("VerwijderAnalyse")]
        public IActionResult VerwijderAnalyseBevestigd(int id)
        {
            try
            {
                Analyse analyse = _analyseRepository.GetById(id);

                if (analyse != null)
                {
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
            }
            catch
            {
                TempData["error"] = "Er ging onverwachts iets fout, probeer later opnieuw.";
            }
            
            return RedirectToAction("Index", "Kairos");
        }
        #endregion

        #region Archiveer
        public IActionResult Archiveer(int id)
        {
            try
            {
                ViewData["analyseId"] = id;
                Analyse analyse = _analyseRepository.GetById(id);

                if (analyse.Departement != null)
                {
                    ViewData["werkgever"] = $"{analyse.Departement.Werkgever.Naam} - {analyse.Departement.Naam}";
                }

                return View("ArchiveerAnalyse");
            }
            catch
            {
                TempData["error"] = "Er ging iets fout tijdens het ophalen van de analyse, probeer later opnieuw";
            }

            return RedirectToAction("Index", "Kairos");
        }

        [HttpPost]
        [ActionName("Archiveer")]
        public IActionResult ArchiveerBevestigd(int id)
        {
            try
            {
                Analyse analyse = _analyseRepository.GetById(id);

                if (analyse != null)
                {
                    // uit archief halen + datum laatste aanpassing aanpassen
                    analyse.InArchief = true;
                    analyse.DatumLaatsteAanpassing = DateTime.Now;

                    // alles opslaan in de databank
                    _analyseRepository.Save();

                    if (analyse.Departement == null)
                    {
                        TempData["message"] = "De analyse is succesvol gearchiveerd.";
                    }
                    else
                    {
                        TempData["message"] =
                            $"De analyse van {analyse.Departement.Werkgever.Naam} - {analyse.Departement.Naam}" +
                            " is succesvol gearchiveerd.";
                    }
                }
                else
                {
                    TempData["error"] = "Er ging onverwachts iets fout, probeer het later opnieuw";
                }
               
            }
            catch
            {
                TempData["error"] = "Er liep iets mis, probeer later opnieuw";
            }

            return RedirectToAction("Index", "Kairos");
        }
        #endregion
    }
}
