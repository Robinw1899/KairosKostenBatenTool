﻿using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    public class BatenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult MedewerkerZelfdeNiveau()
        {
            return RedirectToAction("Index", "MedewerkersZelfdeNiveau");
        }

        public IActionResult MedewerkerHogerNiveau()
        {
            return RedirectToAction("Index", "MedewerkersHogerNiveau");
        }

        public IActionResult UitzendKrachtBesparingen()
        {
            return RedirectToAction("Index", "UitzendKrachtBesparingen");
        }

        public IActionResult ExtraOmzet()
        {
            return RedirectToAction("Index", "ExtraOmzet");
        }

        public IActionResult ExtraProductiviteit()
        {
            return RedirectToAction("Index", "ExtraProductiviteit");
        }

        public IActionResult OverurenBesparingen()
        {
            return RedirectToAction("Index", "OverurenBesparingen");
        }

        public IActionResult ExterneInkopen()
        {
            return RedirectToAction("Index", "ExterneInkopen");
        }

        public IActionResult Subsidies()
        {
            return RedirectToAction("Index", "Subsidies");
        }

        public IActionResult ExtraBesparingen()
        {
            return RedirectToAction("Index", "ExtraBesparingen");
        }
    }
}