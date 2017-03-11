using System;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    public class KairosController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobcoachRepository _gebruikerRepository;
        private readonly IWerkgeverRepository _werkgeverRepository;

        public KairosController(
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager,
            IJobcoachRepository gebruikerRepository,
            IWerkgeverRepository werkgeverRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _gebruikerRepository = gebruikerRepository;
            _werkgeverRepository = werkgeverRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NieuweAnalyse()
        {
            //throw new System.NotImplementedException();
            return View(nameof(NieuweOfBestaandeWerkgever));
        }

        public async Task<IActionResult> EersteKeerAanmelden()
        {
            ApplicationUser user = await _userManager.GetUserAsync(User);
            EersteKeerAanmeldenViewModel model = new EersteKeerAanmeldenViewModel {Email = user.Email};

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EersteKeerAanmelden(EersteKeerAanmeldenViewModel model)
        {
            // Gebruiker meldt eerste keer aan, dus wachtwoord moet ingesteld worden
            ApplicationUser user = await _userManager.GetUserAsync(User);
            string code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var paswoordResetten = await _userManager.ResetPasswordAsync(user, code, model.Password);

            if (paswoordResetten.Succeeded)
            {
                await _signInManager.SignOutAsync();
                var login = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, lockoutOnFailure: false);

                if (login.Succeeded)
                {
                    Jobcoach gebruiker = _gebruikerRepository.GetByEmail(model.Email);
                    gebruiker.AlAangemeld = true;
                    _gebruikerRepository.Save();

                    return RedirectToAction(nameof(Index), "Kairos");
                }
            }

            return RedirectToAction(nameof(AccountController.Login), "Account");
        }
        
        public IActionResult Opmerking()
        {
            // return de view met een OpmerkingViewModel met een leeg onderwerp en leeg bericht
            return View(new OpmerkingViewModel("", ""));
        }

        [HttpPost]
        public async Task<IActionResult> Opmerking(OpmerkingViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // de ingelogde jobcoach ophalen
                    var user = await _userManager.GetUserAsync(User);

                    // gegevens jobcoach ophalen
                    string nameJobcoach = user.Voornaam + " " + user.Naam;
                    string emailJobcoach = user.Email;

                    // mail verzenden
                    EmailSender.SendMailAdmin(nameJobcoach, emailJobcoach, model.Onderwerp, model.Bericht);

                    // als we hier komen, is alles gelukt
                    TempData["message"] =
                        "Je vraag/opmerking is succesvol verzonden naar administrator. Deze zal zo snel mogelijk contact opnemen met jou.";
                    
                    return RedirectToAction(nameof(Index), "Kairos");
                }
                catch (Exception)
                {
                    // er is iets fout gelopen, ga verder en toon de pagina opnieuw
                    TempData["error"] = "Er is onverwacht iets fout gelopen, onze excuses voor het ongemak! " +
                                        "Probeer het later opnieuw.";
                }
                
            }

            // als we hier komen, is er iets mislukt, we tonen de pagina opnieuw
            return View(model);
        }

        //dit wordt opgeroepen als je op de knop BestaandeWerkgever drukt bij de methode NieuweAnalyse
        public IActionResult NieuweAnalyseBestaandeWerkgever(string naam="")
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
                WerkgeverViewModel model = new WerkgeverViewModel();
                return View(model);
            }
        }

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }

        public IActionResult NieuweOfBestaandeWerkgever()//kiezen voor nieuwe of bestaande werkgever
        {
            return View();
        }
    }

}

