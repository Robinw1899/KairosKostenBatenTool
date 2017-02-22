using System;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    public class KairosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NieuweAnalyse()
        {
            throw new System.NotImplementedException();
        }

        public IActionResult Opmerking()
        {
            //throw new System.NotImplementedException();
            return View();
        }

        [HttpPost]
        public IActionResult Opmerking(OpmerkingViewModel opmerkingViewModel)
        {
            throw new NotImplementedException();
            if (ModelState.IsValid)
            {
                try
                {

                }
                catch 
                {
                    
                }
            }
        }
    }
}
