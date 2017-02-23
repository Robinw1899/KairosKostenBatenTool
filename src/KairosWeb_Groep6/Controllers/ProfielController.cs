using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.AccountViewModels;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.ManageViewModels;
using KairosWeb_Groep6.Models.ProfielViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace KairosWeb_Groep6.Controllers
{
    public class ProfielController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobcoachRepository _jobCoachRepository;

        public ProfielController(UserManager<ApplicationUser> userManager, IJobcoachRepository jobCoachRepository)
        {
            _userManager = userManager;
            _jobCoachRepository = jobCoachRepository;
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int id)
        {
            
            Jobcoach jobcoach = _jobCoachRepository.GetById(id);
            if (jobcoach == null)
                return NotFound();

            return View(new ProfielViewModel(jobcoach, jobcoach.Organisatie));

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ProfielViewModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    Jobcoach jobcoach = _jobCoachRepository.GetById(model.JobcoachId);
                    jobcoach.Emailadres = model.Email;
                    jobcoach.Organisatie.Naam = model.OrganisatieNaam;
                    jobcoach.Organisatie.Gemeente = model.Gemeente;
                    jobcoach.Organisatie.Nummer = model.NrOrganisatie;
                    jobcoach.Organisatie.Postcode = model.Postcode;
                    jobcoach.Organisatie.Straat = model.StraatOrganisatie;
                    _jobCoachRepository.SaveChanges();
                    TempData["message"] = "U hebt succesvol uw account gewijzigd.";
                    //return RedirectToLocal(returnUrl);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View(model);

        }
    }
}

