using System;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Filters;
using KairosWeb_Groep6.Models.Domain.Extensions;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    public class KairosController : Controller
    {
        #region Properties
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly IJobcoachRepository _jobcoachRepository;

        private readonly IAnalyseRepository _analyseRepository;
        #endregion

        #region Constructors
        public KairosController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IJobcoachRepository gebruikerRepository,
            IAnalyseRepository analyseRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jobcoachRepository = gebruikerRepository;
            _analyseRepository = analyseRepository;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index(IndexViewModel model = null)
        {
            AnalyseFilter.ClearSession(HttpContext);
            ApplicationUser user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                TempData["error"] = "Gelieve je eerst aan te melden alvorens deze pagina te bezoeken.";
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account");
            }

            try
            {

                Jobcoach jobcoach = _jobcoachRepository.GetByEmail(user.Email);
                List<Analyse> analyses = new List<Analyse>();

                _analyseRepository.SetAnalysesJobcoach(jobcoach, false);
                int totaal = jobcoach.Analyses.Count(); //13


                bool volgende = false;
                bool vorige = false;

                //volgende knop laten zien of niet
                if (totaal > 8 && model.EindIndex < totaal )
                {
                    volgende = true;//true // false
                }

                //vorige knop laten zien of niet
                if (model.BeginIndex != 0)
                {
                    vorige = true;//false //true
                }

                int aantal = 8;
                analyses =  _analyseRepository
                    .GetAnalyses(jobcoach, model.BeginIndex, aantal)
                    .ToList();
           
                jobcoach.Analyses = analyses;

                model = new IndexViewModel(jobcoach)
                {
                    BeginIndex = model.BeginIndex,
                    EindIndex = model.BeginIndex + 8,
                    ShowVolgende = volgende,
                    ShowVorige = vorige
                };
            }
            catch
            {
                TempData["error"] = "Er liep iets mis, probeer later opnieuw";
            }

            return View("Index", model);
        }
        #endregion

        #region VolgendeAnalyses
        public IActionResult Volgende(int beginIndex, int eindIndex)
        {                                   //0             13

            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = eindIndex,     //1
                EindIndex = eindIndex + 8
            };

            return RedirectToAction("Index", model);
        }
        #endregion

        #region VorigeAnalyses
        public IActionResult Vorige(int beginIndex, int eindIndex)
        {

            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = beginIndex - 8,
                EindIndex = beginIndex
            };

            return RedirectToAction("Index", model);
        }
        #endregion

        #region Zoek analyse
        [HttpPost]
        public IActionResult Zoek(string zoekterm)
        {
            try
            {
                string email = HttpContext.User.Identity.Name;
                Jobcoach jobcoach = _jobcoachRepository.GetByEmail(email);

                if (jobcoach != null)
                {
                    jobcoach.SelecteerMatchendeAnalyse(zoekterm);
                    jobcoach.Analyses = jobcoach
                        .Analyses
                        .NietInArchief()
                        .OrderByDescending(t => t.DatumLaatsteAanpassing)
                        .Take(9)
                        .ToList();

                    List<Analyse> analyses = new List<Analyse>();

                    foreach (Analyse a in jobcoach.Analyses)
                    {
                        analyses.Add(_analyseRepository.GetById(a.AnalyseId));
                    }

                    jobcoach.Analyses = analyses;
                }
                // anders worden alle analyses getoond

                IndexViewModel model = new IndexViewModel(jobcoach);              

                ViewData["zoeken"] = "zoeken";
                return View("Index", model);
            }
            catch
            {
                TempData["error"] = "Er liep iets mis, probeer later opnieuw.";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region Eerste keer aanmelden
        public async Task<IActionResult> EersteKeerAanmelden()
        {
            try
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Jobcoach jobcoach = _jobcoachRepository.GetByEmail(user.UserName);
                EersteKeerAanmeldenViewModel model = new EersteKeerAanmeldenViewModel
                {
                    Email = user.Email,
                    AlAangemeld = jobcoach.AlAangemeld
                };

                return View(model);
            }
            catch
            {
                TempData["error"] = "Er ging onverwachts iets is, probeer later opnieuw";
            }

            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> EersteKeerAanmelden(EersteKeerAanmeldenViewModel model)
        {
            try
            {
                // Gebruiker meldt eerste keer aan, dus wachtwoord moet ingesteld worden
                ApplicationUser user = await _userManager.GetUserAsync(User);
                string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var paswoordResetten = await _userManager.ResetPasswordAsync(user, code, model.Password);

                if (paswoordResetten.Succeeded)
                {
                    await _signInManager.SignOutAsync();
                    var login = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

                    if (login.Succeeded)
                    {
                        Jobcoach gebruiker = _jobcoachRepository.GetByEmail(model.Email);
                        gebruiker.AlAangemeld = true;
                        _jobcoachRepository.Save();

                        return RedirectToAction(nameof(Index), "Kairos");
                    }
                }
            }
            catch
            {
                TempData["error"] = "Er liep iets is, probeer later opnieuw";
            }

            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(AccountController.Login), "Account");
        }
        #endregion

        #region Opmerking
        public IActionResult Opmerking()
        {
            OpmerkingViewModel model = new OpmerkingViewModel();
            return View(model);
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
                    string nameJobcoach = user.Voornaam ?? "";
                    string emailJobcoach = user.Email;

                    // mail verzenden
                    bool mailVerzendenGelukt = await EmailSender.SendMailAdmin(nameJobcoach, emailJobcoach, model.Onderwerp, model.Bericht);

                    if (mailVerzendenGelukt)
                    {
                        // als we hier komen, is alles gelukt
                        TempData["message"] =
                            "Je vraag/opmerking is succesvol verzonden naar administrator. Deze zal zo snel mogelijk contact opnemen met jou.";

                        return RedirectToAction(nameof(Index), "Kairos");
                    }
                    else
                    {
                        TempData["error"] = "De opmerking kan momenteel niet verzonden worden, probeer het later opnieuw.";
                    }
                }
                catch
                {
                    // er is iets fout gelopen, ga verder en toon de pagina opnieuw
                    TempData["error"] = "Er is onverwacht iets fout gelopen, onze excuses voor het ongemak! " +
                                        "Probeer het later opnieuw.";
                }
            }

            // als we hier komen, is er iets mislukt, we tonen de pagina opnieuw
            return View(model);
        }
        #endregion
    }
}

