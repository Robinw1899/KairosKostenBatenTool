using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Controllers.Kosten;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KairosWeb_Groep6.Controllers
{
    public class KostenController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BegeleidingsKosten()
        {
            return RedirectToAction("Index", "BegeleidingsKosten");
        }

        public IActionResult EnclaveKosten()
        {
            return RedirectToAction("Index", "EnclaveKosten");
        }

        public IActionResult ExtraKosten()
        {
            return RedirectToAction("Index", "ExtraKosten");
        }

        public IActionResult GereedschapsKosten()
        {
            return RedirectToAction("Index", "GereedschapsKosten");
        }

        public IActionResult InfrastructuurKosten()
        {
            return RedirectToAction("Index", "InfrastructuurKosten");
        }

        public IActionResult Loonkosten()
        {
            return RedirectToAction("Index", "Loonkosten");
        }

        public IActionResult OpleidingsKosten()
        {
            return RedirectToAction("Index", "OpleidingsKosten");
        }

        public IActionResult VoorbereidingsKosten()
        {
            return RedirectToAction("Index", "VoorbereidingsKosten");
        }

    }
}
