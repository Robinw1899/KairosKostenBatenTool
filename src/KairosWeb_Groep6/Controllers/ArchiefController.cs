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
        public IActionResult Index(Jobcoach jobcoach, IndexViewModel model=null)//problemen; aantalShow moet aantal zijn.(aantal analyses en niet aantal op scherm)
        {
            

            try
            {
                if (jobcoach != null)
                {
                    
                    List<Analyse> analysesInArchief = new List<Analyse>();
                   
                   
                 
                    jobcoach.Analyses = jobcoach
                        .Analyses
                        .InArchief()
                        .OrderByDescending(t => t.DatumLaatsteAanpassing)
                        .ToList();

                    int skip = model.AantalKeerSkip;      //0
                    int totaal = jobcoach.Analyses.Count(); //13 
                    int aantal;

                    if (skip != 0)
                    {
                        aantal = model.Aantal;
                    }
                    else if (totaal > 9)
                    {
                        aantal = 9;    //9
                    }
                    else
                    {
                        aantal = totaal;
                    }
                    bool volgende = false;
                    bool vorige = false;

                    //volgende knop laten zien of niet
                    if (totaal > 9 && aantal == 9)
                    {
                        volgende = true;
                    }

                    //vorige knop laten zien of niet
                    if (model.AantalKeerSkip != 0)
                    {
                        vorige = true;
                    }

                    jobcoach.Analyses = jobcoach
                          .Analyses
                          .Skip(skip * 9)  //0  --9
                          .Take(aantal)   //9   --4
                          .ToList();

                    foreach (Analyse a in jobcoach.Analyses.InArchief())
                    {
                        analysesInArchief.Add(_analyseRepository.GetById(a.AnalyseId));
                    }

                    jobcoach.Analyses = analysesInArchief;
               
                    model = new IndexViewModel(jobcoach)
                    {
                        Aantal = totaal,       //13
                        AantalKeerSkip = skip,  //0      
                        showVolgende = volgende,
                        showVorige = vorige
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
        public IActionResult Volgende(int aantalSkips, int aantal)
        {                                   //0             13
            aantalSkips += 1;
            IndexViewModel model = new IndexViewModel
            {
                AantalKeerSkip = aantalSkips,     //1
                Aantal = aantal - (9 * aantalSkips)   //13 - (9*1) -> 4
            };

            return RedirectToAction("Index", model);
        }
        #endregion

        #region VorigeAnalyses
        public IActionResult Vorige(int aantalSkips, int aantal)
        {                               //1             4
            aantalSkips -= 1;   //0
            IndexViewModel model = new IndexViewModel
            {
                AantalKeerSkip = aantalSkips,//0
                Aantal = aantal + (9 * (aantalSkips + 1))   //4 +9*(0+1) -> 13
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

                IndexViewModel model = new IndexViewModel(jobcoach)
                {
                    Aantal = aantalShow
                };

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
