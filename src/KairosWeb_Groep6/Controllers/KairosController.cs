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

        private readonly IJobcoachRepository _gebruikerRepository;

        private readonly IAnalyseRepository _analyseRepository;

        private const int defaultAantal = 9;
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
            _gebruikerRepository = gebruikerRepository;
            _analyseRepository = analyseRepository;
        }
        #endregion

        public async Task<IActionResult> Index(IndexViewModel model = null)
        {
            AnalyseFilter.ClearSession(HttpContext);
            ApplicationUser user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                TempData["error"] = "Gelieve je eerst aan te melden alvorens deze pagina te bezoeken.";
                model = new IndexViewModel();
            }
            else
            {
                int aantal = 9;

                if (model != null)
                {
                    aantal = model.Aantal == 0 ? 9 : model.Aantal;
                }

                Jobcoach jobcoach = _gebruikerRepository.GetByEmail(user.Email);
                List<Analyse> analyses = new List<Analyse>();
                jobcoach.Analyses = jobcoach
                    .Analyses
                    .NietInArchief()
                    .OrderByDescending(t => t.DatumLaatsteAanpassing)
                    .Take(aantal)
                    .ToList();

                foreach (Analyse a in jobcoach.Analyses)
                {
                    analyses.Add(_analyseRepository.GetById(a.AnalyseId));
                }

                jobcoach.Analyses = analyses;

                model = new IndexViewModel(jobcoach)
                {
                    Aantal = aantal
                };
            }

            return View("Index", model);
        }

        #region Toon meer
        public IActionResult ToonMeer(int id)
        {
            // id is aantal
            IndexViewModel model = new IndexViewModel
            {
                Aantal = id + 9
            };

            return RedirectToAction("Index", model);
        }
        #endregion

        #region Zoek

        [HttpPost]
        public IActionResult Zoek(string zoekterm)
        {
            string email = HttpContext.User.Identity.Name;
            Jobcoach jobcoach = _gebruikerRepository.GetByEmail(email);

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

            IndexViewModel model = new IndexViewModel(jobcoach)
            {
                Aantal = 9
            };

            ViewData["zoeken"] = "zoeken";
            return View("Index", model);
        }
        #endregion

        #region Eerste keer aanmelden       
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
                var login = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);

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
        #endregion

        #region Opmerking
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
        #endregion

        private bool IsAjaxRequest()
        {
            return Request != null && Request.Headers["X-Requested-With"] == "XMLHttpRequest";
        }
    }
}

