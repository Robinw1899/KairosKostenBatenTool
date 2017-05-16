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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    [AutoValidateAntiforgeryToken]
    public class KairosController : Controller
    {
        #region Properties
        private const int MAX_AANTAL_ANALYSES = 9;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobcoachRepository _jobcoachRepository;
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;
        #endregion

        #region Constructors
        public KairosController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IJobcoachRepository gebruikerRepository,
            IAnalyseRepository analyseRepository,
            IExceptionLogRepository exceptionLogRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jobcoachRepository = gebruikerRepository;
            _analyseRepository = analyseRepository;
            _exceptionLogRepository = exceptionLogRepository;
        }
        #endregion

        #region Index
        public async Task<IActionResult> Index()
        {
            try
            {
                if (HttpContext != null)
                {
                    AnalyseFilter.ClearSession(HttpContext);
                }

                ApplicationUser user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    TempData["error"] = "Gelieve je eerst aan te melden alvorens deze pagina te bezoeken.";
                    await _signInManager.SignOutAsync();
                    return RedirectToAction("Login", "Account");
                }

                IndexViewModel model = new IndexViewModel
                {
                    BeginIndex = 0,
                    EindIndex = MAX_AANTAL_ANALYSES
                };

                return View("Index", model);
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Kairos", "Index"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Je analyses konder niet geladen worden, probeer later opnieuw";
            }

            return View("Index", new IndexViewModel());
        }
        #endregion

        #region HaalAnalysesOp
        public IActionResult HaalAnalysesOpZonderModel(int beginIndex, int eindIndex)
        {
            // methode om het IndexViewmodel te kunnen aanmaken
            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = beginIndex,
                EindIndex = eindIndex
            };

            return RedirectToAction("HaalAnalysesOp", model);
        }

        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult HaalAnalysesOp(Jobcoach jobcoach, IndexViewModel model = null)
        {
            try
            {              
                _analyseRepository.SetAnalysesJobcoach(jobcoach, false);
                int totaal = jobcoach.Analyses.Count; //13

                bool volgende = false;
                bool vorige = false;
               
                //volgende knop laten zien of niet
                if (totaal > MAX_AANTAL_ANALYSES && model?.EindIndex < totaal)
                {
                    volgende = true;//true // false
                }

                //vorige knop laten zien of niet
                if (model?.BeginIndex != 0)
                {
                    vorige = true;//false //true
                }

                int aantal = MAX_AANTAL_ANALYSES;
                var analyses = _analyseRepository
                    .GetAnalyses(jobcoach, model.BeginIndex, aantal)
                    .ToList();

                jobcoach.Analyses = analyses;

                model = new IndexViewModel(jobcoach)
                {
                    BeginIndex = model.BeginIndex,
                    EindIndex = model.BeginIndex + MAX_AANTAL_ANALYSES,
                    ShowVolgende = volgende,
                    ShowVorige = vorige                   
                };

                return PartialView("_Analyses", model);
            }
            catch(Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Kairos", "HaalAnalysesOp"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er liep iets mis, probeer later opnieuw";
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region VolgendeAnalyses
        public IActionResult Volgende(int beginIndex, int eindIndex)
        {                                   //0             13

            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = eindIndex,     //1
                EindIndex = eindIndex + MAX_AANTAL_ANALYSES
            };

            return RedirectToAction("HaalAnalysesOp", model);
        }
        #endregion

        #region VorigeAnalyses
        public IActionResult Vorige(int beginIndex, int eindIndex)
        {

            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = beginIndex - MAX_AANTAL_ANALYSES,
                EindIndex = beginIndex
            };

            return RedirectToAction("HaalAnalysesOp", model);
        }
        #endregion

        #region Zoek analyse
        [HttpPost]
        [ServiceFilter(typeof(JobcoachFilter))]
        public IActionResult Zoek(Jobcoach jobcoach, string zoekterm)
        {
            try
            {
                //string email = HttpContext.User.Identity.Name;
                //Jobcoach jobcoach = _jobcoachRepository.GetByEmail(email);

                if (jobcoach != null)
                {
                    jobcoach.SelecteerMatchendeAnalyse(zoekterm);
                    jobcoach.Analyses = jobcoach
                        .Analyses
                        .NietInArchief()
                        .OrderByDescending(t => t.DatumLaatsteAanpassing)
                        //.Take(9)
                        .ToList();
                }

                IndexViewModel model = new IndexViewModel(jobcoach);
                IEnumerable<Datum> datumTypes = Enum.GetValues(typeof(Datum))
                                                    .Cast<Datum>();

                model.listItems = from date in datumTypes
                                  select new SelectListItem
                                  {
                                      Text = date.ToString(),
                                      Value = ((int)date).ToString()
                                  };

                ViewData["zoeken"] = "zoeken";
                return PartialView("_Analyses",model);
               
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Kairos", "Zoek"));
                _exceptionLogRepository.Save();
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
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Kairos", "EersteKeerAanmelden -- GET --"));
                _exceptionLogRepository.Save();
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
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Kairos", "EersteKeerAanmelden -- POST --"));
                _exceptionLogRepository.Save();
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
                catch (Exception e)
                {
                    _exceptionLogRepository.Add(new ExceptionLog(e, "Kairos", "Opmerking"));
                    _exceptionLogRepository.Save();
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

