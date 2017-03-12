﻿using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    public class AnalyseController : Controller
    {
        #region Properties

        private readonly IWerkgeverRepository _werkgeverRepository;
        #endregion

        #region Constructors

        public AnalyseController(IWerkgeverRepository werkgeverRepository)
        {
            _werkgeverRepository = werkgeverRepository;
        }
        #endregion

        public IActionResult Index()
        {
            return RedirectToAction("Index", "Baten");
        }

        public IActionResult NieuweAnalyse()
        {// hier word gekozen tussen een nieuwe of bestaande werkgever
            return View();
        }

        public IActionResult NieuweWerkgever()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NieuweWerkgever(int id)
        {
            return View();
        }

        public IActionResult BestaandeWerkgever(string naam = "")
        {
            if (naam.Equals(""))
                ViewData["Werkgevers"] = _werkgeverRepository.GetAll();
            else
            {
                ViewData["Werkgevers"] = _werkgeverRepository.GetByName(naam);
            }
            if (IsAjaxRequest())
                return PartialView("_Werkgevers");
            else
            {
                //WerkgeverViewModel model = new WerkgeverViewModel();
                return View();
            }
        }

        [HttpPost]
        public IActionResult BestaandeWerkgever(int id)
        {
            return View();
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}
