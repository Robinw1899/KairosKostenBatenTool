using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KairosWeb_Groep6.Controllers
{
    public class BatenController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ExterneInkoop()
        {
            return View();
        }
        public IActionResult ExtraOmzet()
        {
            return View();
        }
        public IActionResult ExtraProductiviteit()
        {
            return View();
        }
        public IActionResult MedewerkerNiveauBaat()
        {
            return View();
        }
        public IActionResult OverurenBesparing()
        {
            return View();
        }
        public IActionResult UitzendKracht()
        {
            return View();
        }
        public IActionResult ExtraBesparing()
        {
            return View();
        }
        public void Opslaan()
        {
            throw new NotImplementedException();
        }

        public void Bewerken()
        {
            throw new NotImplementedException();
        }
    }
}
