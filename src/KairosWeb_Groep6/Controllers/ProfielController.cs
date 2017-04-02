using System;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.ProfielViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    public class ProfielController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobcoachRepository _gebruikerRepository;

        public ProfielController(
            UserManager<ApplicationUser> userManager, 
            IJobcoachRepository gebruikerRepository)
        {
            _userManager = userManager;
            _gebruikerRepository = gebruikerRepository;
        }

        public IActionResult Index()
        {
            Jobcoach gebruiker = _gebruikerRepository.GetByEmail(_userManager.GetUserAsync(User).Result.Email);

            return View(new ProfielViewModel(gebruiker));
        }

        [HttpPost]
        public async Task<IActionResult> Index(ProfielViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.OrganisatieNaam == null)
                    {
                        // Admin heeft gegevens gewijzigd
                        Jobcoach gebruiker = _gebruikerRepository.GetById(model.GebruikerId);
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
                        Jobcoach jobcoach = _gebruikerRepository.GetById(model.GebruikerId);
                        jobcoach.Emailadres = model.Email;
                        jobcoach.Organisatie.Naam = model.OrganisatieNaam;
                        jobcoach.Organisatie.Gemeente = model.Gemeente;

                        if (model.NrOrganisatie != null)
                            jobcoach.Organisatie.Nummer = (int)model.NrOrganisatie;

                        if (model.Postcode != null)
                            jobcoach.Organisatie.Postcode = (int)model.Postcode;

                        jobcoach.Organisatie.Straat = model.StraatOrganisatie;
                        jobcoach.Organisatie.Bus = model.BusOrganisatie;

                        var user = await _userManager.GetUserAsync(User);
                        string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                        await _userManager.ChangeEmailAsync(user, model.Email, token);

                        _gebruikerRepository.Save();
                    }

                    TempData["message"] = "Uw profiel is succesvol gewijzigd.";

                    return RedirectToAction(nameof(Index));
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
