using System;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class ArchiefController : Controller
    {
        #region Properties
        private const int MAX_AANTAL_ANALYSES = 9;
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;
        #endregion

        #region Constructors
        public ArchiefController(IAnalyseRepository analyseRepository,
            IExceptionLogRepository exceptionLogRepository)
        {
            _analyseRepository = analyseRepository;
            _exceptionLogRepository = exceptionLogRepository;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = 0,
                EindIndex = MAX_AANTAL_ANALYSES
            };
            IEnumerable<Datum> datumTypes = Enum.GetValues(typeof(Datum))
                                              .Cast<Datum>();

            model.listItems = from date in datumTypes
                              select new SelectListItem
                              {
                                  Text = ((int)date) > 1 ? ((int)date).ToString() + " maanden" : ((int)date).ToString() + " maand",
                                  Value = ((int)date).ToString()
                              };


            return View("Index", model);
        }
        #endregion

        #region HaalAnalysesOp
        public IActionResult HaalAnalysesOpZonderModel(int beginIndex, int eindIndex)
        {
            // methode om het IndexViewmodel te kunnen aanmaken
            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = beginIndex,
                EindIndex = eindIndex
            };

            return RedirectToAction("HaalAnalysesOp", model);
        }

        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult HaalAnalysesOp(Jobcoach jobcoach, IndexViewModel model = null)
        {
            try
            {
                _analyseRepository.SetAnalysesJobcoach(jobcoach, true);
                int totaal = jobcoach.Analyses.Count; //13

                bool volgende = false;
                bool vorige = false;

                //volgende knop laten zien of niet
                if (totaal > MAX_AANTAL_ANALYSES && model?.EindIndex < totaal)
                {
                    volgende = true;//true // false
                }

                //vorige knop laten zien of niet
                if (model?.BeginIndex != 0)
                {
                    vorige = true;//false //true
                }

                int aantal = MAX_AANTAL_ANALYSES;
                var analyses = _analyseRepository
                    .GetAnalyses(jobcoach, model.BeginIndex, aantal)
                    .ToList();

                jobcoach.Analyses = analyses;

                model = new IndexViewModel(jobcoach)
                {
                    BeginIndex = model.BeginIndex,
                    EindIndex = model.BeginIndex + MAX_AANTAL_ANALYSES,
                    ShowVolgende = volgende,
                    ShowVorige = vorige
                };

                return PartialView("_Analyses", model);
            }
            catch(Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Archief", "HaalAnalysesOp"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er liep iets mis, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region VolgendeAnalyses
        public IActionResult Volgende(int beginIndex, int eindIndex)
        {                                   //0             13

            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = eindIndex,     //1
                EindIndex = eindIndex + MAX_AANTAL_ANALYSES
            };

            return RedirectToAction("HaalAnalysesOp", model);
        }
        #endregion

        #region VorigeAnalyses
        public IActionResult Vorige(int beginIndex, int eindIndex)
        {

            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = beginIndex - MAX_AANTAL_ANALYSES,
                EindIndex = beginIndex
            };

            return RedirectToAction("HaalAnalysesOp", model);
        }
        #endregion

        #region Zoek analyse
        [HttpPost]
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult Zoek(Jobcoach jobcoach, string zoekterm)
        {
            try
            {
                if (jobcoach != null)
                {
                    jobcoach.SelecteerMatchendeAnalyse(zoekterm);
                    jobcoach.Analyses = jobcoach
                        .Analyses
                        .InArchief()
                        .OrderByDescending(t => t.DatumLaatsteAanpassing)
                        .ToList();
                }

                IndexViewModel model = new IndexViewModel(jobcoach);           

                ViewData["zoeken"] = "zoeken";
                return PartialView("_Analyses", model);
            }
            catch(Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Arhief", "Zoek"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er ging onverwacht iets fout, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Datumsearch   
        [HttpPost]
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult ZoekDatum(string val, Jobcoach jobcoach)
        {
            try
            {
                if (jobcoach != null)
                {

                    jobcoach.SelecteedMatchendeAnalyseDatum(val);
                    jobcoach.Analyses = jobcoach
                           .Analyses
                           .InArchief()
                           .OrderByDescending(t => t.DatumLaatsteAanpassing)
                           .ToList();
                    IndexViewModel model = new IndexViewModel(jobcoach);

                    ViewData["zoeken"] = "zoeken";
                    return PartialView("_Analyses", model);
                }
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Kairos", "DropDownchange"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er liep iets mis, probeer later opnieuw.";
            }
            return RedirectToAction("Index");
        }
        #endregion

        #region HaalAnalyseUitArchief
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult HaalAnalyseUitArchief(Jobcoach jobcoach, int id)
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

                    return View("HaalAnalyseUitArchief");
                }
            }
            catch(Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Archief", "HaalAnalyseUitArchief"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er ging iets mis tijdens het ophalen van de analyse, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("HaalAnalyseUitArchief")]
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult HaalAnalyseUitArchiefBevestigd(Jobcoach jobcoach, int id)
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

                    // uit archief halen + datum laatste aanpassing aanpassen
                    analyse.InArchief = false;
                    analyse.DatumLaatsteAanpassing = DateTime.Now;

                    // alles opslaan in de databank
                    _analyseRepository.Save();

                    if (analyse.Departement == null)
                    {
                        TempData["message"] = "De analyse is succesvol uit het archief gehaald.";
                    }
                    else
                    {
                        TempData["message"] = $"De analyse van {analyse.Departement.Werkgever.Naam} - {analyse.Departement.Naam}" +
                                              " is succesvol uit het archief gehaald.";
                    }
                }
            }
            catch(Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Archief", "HaalAnalyseUitArchiefBevestigd"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er ging onverwacht iets fout, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region OpenAnalyse
        public IActionResult OpenAnalyse(int id)
        {
            return RedirectToAction("OpenAnalyse", "Analyse",  id);
        }
        #endregion

        #region VerwijderAnalyse
        public IActionResult VerwijderAnalyse(int id)
        {
            return RedirectToAction("VerwijderAnalyse", "Analyse", id);
        }
        #endregion

        #region MaakExcelAnalyse
        public IActionResult MaakExcelAnalyse(int id)
        {
            return RedirectToAction("MaakExcel", "Resultaat", id);
        }
        #endregion

        #region MailAnalyse
        public IActionResult MailAnalyse(int id)
        {
            return RedirectToAction("Mail", "Resultaat", id);
        }
        #endregion

        #region ToonMeer
        public IActionResult Volgende9(int aantalSkip)
        {
            aantalSkip += 1;

           return  RedirectToAction("Index", new { aantalSkip  = aantalSkip });
        }
        #endregion
    }
}
