using System;
using System.Linq;
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
        private readonly IExceptionLogRepository _exceptionLogRepository;
        #endregion

        #region Constructors
        public AnalyseController(
            IAnalyseRepository analyseRepository,
            IJobcoachRepository jobcoachRepository,
            IExceptionLogRepository exceptionLogRepository)
        {
            _analyseRepository = analyseRepository;
            _jobcoachRepository = jobcoachRepository;
            _exceptionLogRepository = exceptionLogRepository;
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
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Analyse", "NieuweAnalyse"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er ging iets fout tijdens het starten van een nieuwe analyse, probeer later opnieuw";
            }

            return RedirectToAction("Index", "Kairos");
        }
        #endregion

        #region OpenAnalyse
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult OpenAnalyse(Jobcoach jobcoach, int id)
        {
            try
            {
                // eerst kijken of deze analyse wel van deze jobcoach is
                Analyse mogelijkeAnalyse = jobcoach.Analyses.SingleOrDefault(a => a.AnalyseId == id);

                if (mogelijkeAnalyse == null || mogelijkeAnalyse.Verwijderd)
                {
                    TempData["error"] = "U heeft geen toegang tot deze analyse! Open enkel analyses die u ziet " +
                                        "op de homepagina of in het archief.";
                }
                else
                {
                    Analyse analyse = _analyseRepository.GetById(id);

                    if (HttpContext != null)
                    {
                        // nodig voor testen, HttpContext kan je niet mocken
                        // analyse instellen in Session
                        AnalyseFilter.SetAnalyseInSession(HttpContext, analyse);
                    }

                    return RedirectToAction("Index", "Resultaat");
                }
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Analyse", "OpenAnalyse"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er ging iets fout tijdens het ophalen van de analyse, probeer later opnieuw";
            }

            return RedirectToAction("Index", "Kairos");
        }
        #endregion

        #region VerwijderAnalyse
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult VerwijderAnalyse(Jobcoach jobcoach, int id, string from)
        {
            try
            {
                // eerst kijken of deze analyse wel van deze jobcoach is
                Analyse mogelijkeAnalyse = jobcoach.Analyses.SingleOrDefault(a => a.AnalyseId == id);

                if (mogelijkeAnalyse == null || mogelijkeAnalyse.Verwijderd)
                {
                    TempData["error"] = "U heeft geen toegang tot deze analyse! Verwijder enkel analyses die u ziet " +
                                        "op de homepagina of in het archief.";
                }
                else
                {
                    Analyse analyse = _analyseRepository.GetById(id);

                    if (analyse != null)
                    {
                        ViewData["analyseId"] = id;

                        if (analyse.Departement != null)
                        {
                            ViewData["werkgever"] = $"{analyse.Departement.Werkgever.Naam} - {analyse.Departement.Naam}";
                        }

                        ViewData["returnUrl"] = from;

                        return View();
                    }
                }
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Analyse", "VerwijderAnalyse"));
                _exceptionLogRepository.Save();
                TempData["error"] =
                    "Er ging iets fout tijdens het voorbereiden van het verwijderen, probeer later opnieuw";
            }

            return RedirectToAction("Index", "Kairos");
        }

        [HttpPost]
        [ActionName("VerwijderAnalyse")]
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult VerwijderAnalyseBevestigd(Jobcoach jobcoach, int id)
        {
            try
            {
                // eerst kijken of deze analyse wel van deze jobcoach is
                Analyse mogelijkeAnalyse = jobcoach.Analyses.SingleOrDefault(a => a.AnalyseId == id);

                if (mogelijkeAnalyse == null || mogelijkeAnalyse.Verwijderd)
                {
                    TempData["error"] = "U heeft geen toegang tot deze analyse! Verwijderd enkel analyses die u ziet " +
                                        "op de homepagina of in het archief.";
                }
                else
                {
                    Analyse analyse = _analyseRepository.GetById(id);

                    if (analyse != null)
                    {
                        analyse.Verwijderd = true;

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
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Analyse", "VerwijderAnalyseBevestigd"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er ging onverwachts iets fout, probeer later opnieuw.";
            }

            return RedirectToAction("Index", "Kairos");
        }
        #endregion

        #region Archiveer
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult Archiveer(Jobcoach jobcoach, int id)
        {
            try
            {
                // eerst kijken of deze analyse wel van deze jobcoach is
                Analyse mogelijkeAnalyse = jobcoach.Analyses.SingleOrDefault(a => a.AnalyseId == id);

                if (mogelijkeAnalyse == null || mogelijkeAnalyse.Verwijderd)
                {
                    TempData["error"] = "U heeft geen toegang tot deze analyse! Open enkel analyses die u ziet " +
                                        "op de homepagina of in het archief.";
                }
                else
                {
                    ViewData["analyseId"] = id;
                    Analyse analyse = _analyseRepository.GetById(id);

                    if (analyse.Departement != null)
                    {
                        ViewData["werkgever"] = $"{analyse.Departement.Werkgever.Naam} - {analyse.Departement.Naam}";
                    }

                    return View("ArchiveerAnalyse");
                }
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Analyse", "Archiveer"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er ging iets fout tijdens het ophalen van de analyse, probeer later opnieuw";
            }

            return RedirectToAction("Index", "Kairos");
        }

        [HttpPost]
        [ActionName("Archiveer")]
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult ArchiveerBevestigd(Jobcoach jobcoach, int id)
        {
            try
            {
                // eerst kijken of deze analyse wel van deze jobcoach is
                Analyse mogelijkeAnalyse = jobcoach.Analyses.SingleOrDefault(a => a.AnalyseId == id);

                if (mogelijkeAnalyse == null || mogelijkeAnalyse.Verwijderd)
                {
                    TempData["error"] = "U heeft geen toegang tot deze analyse! Open enkel analyses die u ziet " +
                                        "op de homepagina of in het archief.";
                }
                else
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
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Analyse", "ArchiveerBevestigd"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er liep iets mis, probeer later opnieuw";
            }

            return RedirectToAction("Index", "Kairos");
        }
        #endregion
    }
}
