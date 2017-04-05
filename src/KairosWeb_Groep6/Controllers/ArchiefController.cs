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
        #endregion

        #region Constructors
        public ArchiefController(IAnalyseRepository analyseRepository)
        {
            _analyseRepository = analyseRepository;
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
                    IEnumerable<Analyse> analysesInArchief = jobcoach.Analyses.InArchief();
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
                ErrorViewModel errorViewModel = new ErrorViewModel {Exception = e};
                return View("Error", errorViewModel);
            }

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
                    TempData["message"] = "De analyse is succesvol verwijderd.";
                }
                else
                {
                    TempData["message"] = $"De analyse van {analyse.Departement.Werkgever.Naam} - {analyse.Departement.Naam}" +
                                          " is succesvol verwijderd.";
                }
            }
            catch (Exception e)
            {
                TempData["error"] = e.Message;
                ErrorViewModel errorViewModel = new ErrorViewModel { Exception = e };
                return View("Error", errorViewModel);
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