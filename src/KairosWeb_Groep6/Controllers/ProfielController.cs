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
        private readonly IGebruikerRepository _gebruikerRepository;
        private readonly IJobcoachRepository _jobCoachRepository;

        public ProfielController(
            UserManager<ApplicationUser> userManager, 
            IJobcoachRepository jobCoachRepository,
            IGebruikerRepository gebruikerRepository)
        {
            _userManager = userManager;
            _jobCoachRepository = jobCoachRepository;
            _gebruikerRepository = gebruikerRepository;
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        public IActionResult Index()
        {
            Gebruiker gebruiker = _gebruikerRepository.GetBy(_userManager.GetUserAsync(User).Result.Email);

            if (!gebruiker.IsAdmin)
            {
                Jobcoach jobcoach = _jobCoachRepository.GetByEmail(_userManager.GetUserName(User));
                if (jobcoach == null)
                    return NotFound();

                return View(new ProfielViewModel(jobcoach));
            }

            return View(new ProfielViewModel(gebruiker));
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfielViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.OrganisatieNaam == null)
                    {
                        // Admin heeft gegevens gewijzigd
                        Gebruiker gebruiker = _gebruikerRepository.GetById(model.GebruikerId);
                        gebruiker.Naam = model.Naam;
                        gebruiker.Voornaam = model.Voornaam;
                        gebruiker.Emailadres = model.Email;

                        var user = await _userManager.GetUserAsync(User);
                        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        await _userManager.ChangeEmailAsync(user, model.Email, token);

                        _gebruikerRepository.Save();
                    }
                    else
                    {
                        // Jobcoach heeft gegevens gewijzigd
                        Jobcoach jobcoach = _jobCoachRepository.GetById(model.GebruikerId);
                        jobcoach.Emailadres = model.Email;
                        jobcoach.Organisatie.Naam = model.OrganisatieNaam;
                        jobcoach.Organisatie.Gemeente = model.Gemeente;

                        if (model.NrOrganisatie != null)
                            jobcoach.Organisatie.Nummer = (int)model.NrOrganisatie;

                        if (model.Postcode != null)
                            jobcoach.Organisatie.Postcode = (int)model.Postcode;

                        jobcoach.Organisatie.Straat = model.StraatOrganisatie;

                        var user = await _userManager.GetUserAsync(User);
                        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        await _userManager.ChangeEmailAsync(user, model.Email, token);

                        _jobCoachRepository.Save();
                    }

                    TempData["message"] = "Uw profiel is succesvol gewijzigd.";

                    return RedirectToAction(nameof(KairosController.Index), "Kairos");
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
