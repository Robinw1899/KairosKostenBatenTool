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
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobcoachRepository _gebruikerRepository;

        public ProfielController(
            UserManager<ApplicationUser> userManager, 
            IJobcoachRepository gebruikerRepository,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _gebruikerRepository = gebruikerRepository;
        }

        public IActionResult Index()
        {
            Jobcoach gebruiker = _gebruikerRepository.GetByEmail(HttpContext.User.Identity.Name);

            return View(new ProfielViewModel(gebruiker));
        }

        [HttpPost]
        public async Task<IActionResult> Opslaan(ProfielViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
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

                    if (!model.Email.Equals(HttpContext.User.Identity.Name))
                    {// email is gewijzigd
                        var user = await _userManager.GetUserAsync(User);
                        string token = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email);

                        await _userManager.ChangeEmailAsync(user, model.Email, token);

                        user.UserName = model.Email;

                        await _userManager.UpdateAsync(user);

                        // Dus opnieuw inloggen
                        await _signInManager.SignOutAsync();
                        TempData["message"] =
                            "Je hebt je e-mailadres gewijzigd. Gelieve opnieuw in te loggen met dit e-mailadres, " +
                            "zo is alles correct gewijzigd.";

                        return RedirectToAction(nameof(Index));
                    }

                    _gebruikerRepository.Save();

                    TempData["message"] = "Uw profiel is succesvol gewijzigd.";

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }

            return RedirectToAction("Index");
        }
    }
}
