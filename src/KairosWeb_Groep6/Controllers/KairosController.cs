using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.AccountViewModels;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    public class KairosController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGebruikerRepository _gebruikerRepository;
        private readonly IWerkgeverRepository _werkgeverRepository;

        public KairosController(
            SignInManager<ApplicationUser> signInManager, 
            UserManager<ApplicationUser> userManager,
            IGebruikerRepository gebruikerRepository,
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
            throw new System.NotImplementedException();
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
                    Gebruiker gebruiker = _gebruikerRepository.GetByEmail(model.Email);
                    gebruiker.AlAangemeld = true;
                    _gebruikerRepository.Save();

                    return RedirectToAction(nameof(KairosController.Index), "Kairos");
                }
            }

            return RedirectToAction(nameof(AccountController.Login), "Account");
        }
        
        public IActionResult Opmerking()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Opmerking(OpmerkingViewModel opmerkingViewModel)
        {
            throw new NotImplementedException();
        }

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


    }

}

