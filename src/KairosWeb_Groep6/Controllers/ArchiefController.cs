﻿using System;
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

        #region Methods
        public IActionResult Index(Jobcoach jobcoach)
        {
            IndexViewModel model = new IndexViewModel();

            try
            {
                if (jobcoach != null)
                {
                    List<Analyse> analysesInArchief = new List<Analyse>();

                    foreach (Analyse a in jobcoach.Analyses.InArchief())
                    {
                        analysesInArchief.Add(_analyseRepository.GetById(a.AnalyseId));
                    }

                    jobcoach.Analyses = analysesInArchief;

                    List<AnalyseViewModel> viewModels = analysesInArchief.Select(a => new AnalyseViewModel(a)).ToList();
                    model.Analyses = viewModels;
                }
                else
                {
                    TempData["error"] = "Gelieve eerst in te loggen alvorens deze pagina te bezoeken.";
                }
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return View("Index", model);
        }


        [HttpPost]
        public IActionResult Zoek(string zoekterm)
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
                    .Take(9)
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
                Aantal = 9
            };

            ViewData["zoeken"] = "zoeken";
            return View("Index", model);
        }

        public IActionResult HaalAnalyseUitArchief(int id)
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
            catch (Exception e)
            {
                TempData["error"] = e.Message;
            }

            return RedirectToAction("Index");
        }

        public IActionResult OpenAnalyse(int id)
        {
            return RedirectToAction("OpenAnalyse", "Analyse",  id);
        }

        public IActionResult VerwijderAnalyse(int id)
        {
            return RedirectToAction("VerwijderAnalyse", "Analyse", id);
        }

        public IActionResult MaakExcelAnalyse()
        {
            throw new NotImplementedException("Archief/MaakExcelAnalyse");
        }

        public IActionResult MaakPdfAnalyse()
        {
            throw new NotImplementedException("Archief/MaakPdfAnalyse");
        }

        public IActionResult AfdrukkenAnalyse()
        {
            throw new NotImplementedException("Archief/AfdrukkenAnalyse");
        }

        public IActionResult MailAnalyse()
        {
            throw new NotImplementedException("Archief/MailAnalyse");
        }
        #endregion
    }
}
