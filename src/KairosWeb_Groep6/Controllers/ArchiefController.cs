using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    [ServiceFilter(typeof(JobcoachFilter))]
    public class ArchiefController : Controller
    {
        #region Properties
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IJobcoachRepository _gebruikerRepository;
        #endregion

        #region Constructors
        public ArchiefController(IAnalyseRepository analyseRepository,
            IJobcoachRepository gebruikerRepository)
        {
            _analyseRepository = analyseRepository;
            _gebruikerRepository = gebruikerRepository;
        }
        #endregion

        #region Index
        public IActionResult Index(Jobcoach jobcoach, IndexViewModel model = null)//problemen; aantalShow moet aantal zijn.(aantal analyses en niet aantal op scherm)
        {


            try
            {
                if (jobcoach != null)
                {

                    List<Analyse> analysesInArchief = new List<Analyse>();

                    _analyseRepository.SetAnalysesJobcoach(jobcoach, true);
                    int totaal = jobcoach.Analyses.Count(); //13

                    bool volgende = false;
                    bool vorige = false;

                    //volgende knop laten zien of niet
                    if (totaal > 9 && model.EindIndex < totaal )
                    {
                        volgende = true;//true // false
                    }

                    //vorige knop laten zien of niet
                    if (model.BeginIndex != 0)
                    {
                        vorige = true;//false //true
                    }
                    //controle voor het correct aantal te taken
                    int aantal = 9;

                    analysesInArchief = _analyseRepository
                            .GetAnalyses(jobcoach, model.BeginIndex, aantal)
                            .ToList();

                    jobcoach.Analyses = analysesInArchief;
                    model = new IndexViewModel(jobcoach)
                    {
                        BeginIndex = model.BeginIndex,
                        EindIndex = model.BeginIndex + 9,
                        ShowVolgende = volgende,
                        ShowVorige = vorige
                    };

                }
                else
                {
                    TempData["error"] = "Gelieve eerst in te loggen alvorens deze pagina te bezoeken.";
                }
            }
            catch
            {
                TempData["error"] = "Er ging onverwacht iets fout, probeer later opnieuw";
            }

            return View("Index", model);
        }
        #endregion

        #region VolgendeAnalyses
        public IActionResult Volgende(int beginIndex, int eindIndex)
        {                                   //0             13

            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = eindIndex,     //1
                EindIndex = eindIndex + 9
            };

            return RedirectToAction("Index", model);
        }
        #endregion

        #region VorigeAnalyses
        public IActionResult Vorige(int beginIndex, int eindIndex)
        {

            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = beginIndex - 9,
                EindIndex = beginIndex
            };

            return RedirectToAction("Index", model);
        }
        #endregion

        #region Zoek analyse
        [HttpPost]
        public IActionResult Zoek(string zoekterm,int aantalShow=9)
        {
            try
            {
                string email = HttpContext.User.Identity.Name;
                Jobcoach jobcoach = _gebruikerRepository.GetByEmail(email);

                if (jobcoach != null)
                {
                    jobcoach.SelecteerMatchendeAnalyse(zoekterm);
                    jobcoach.Analyses = jobcoach
                        .Analyses
                        .InArchief()
                        .OrderByDescending(t => t.DatumLaatsteAanpassing)
                        .Take(aantalShow)
                        .ToList();

                    List<Analyse> analyses = new List<Analyse>();

                    foreach (Analyse a in jobcoach.Analyses)
                    {
                        analyses.Add(_analyseRepository.GetById(a.AnalyseId));
                    }

                    jobcoach.Analyses = analyses;
                }

                IndexViewModel model = new IndexViewModel(jobcoach);           

                ViewData["zoeken"] = "zoeken";
                return View("Index", model);
            }
            catch
            {
                TempData["error"] = "Er ging onverwacht iets fout, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region HaalAnalyseUitArchief
        public IActionResult HaalAnalyseUitArchief(int id)
        {
            try
            {
                ViewData["analyseId"] = id;
                Analyse analyse = _analyseRepository.GetById(id);

                if (analyse.Departement != null)
                {
                    ViewData["werkgever"] = $"{analyse.Departement.Werkgever.Naam} - {analyse.Departement.Naam}";
                }

                return View("HaalAnalyseUitArchief");
            }
            catch
            {
                TempData["error"] = "Er ging iets mis tijdens het ophalen van de analyse, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("HaalAnalyseUitArchief")]
        public IActionResult HaalAnalyseUitArchiefBevestigd(int id)
        {
            try
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
            catch
            {
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

        #region MaakPdfAnalyse
        public IActionResult MaakPdfAnalyse(int id)
        {
            throw new NotImplementedException("Archief/MaakPdfAnalyse");
        }
        #endregion

        #region AfdrukkenAnalyse
        public IActionResult AfdrukkenAnalyse(int id)
        {
            throw new NotImplementedException("Archief/AfdrukkenAnalyse");
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

           return  RedirectToAction("Index", new { aantalSkip = aantalSkip});
        }
        #endregion
    }
}
