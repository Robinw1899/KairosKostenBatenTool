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
    [AutoValidateAntiforgeryToken]
    public class ProfielController : Controller
    {
        #region Properties
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobcoachRepository _gebruikerRepository;
        private readonly IExceptionLogRepository _exceptionLogRepository;
        private readonly IOrganisatieRepository _organisatieRepository;
        #endregion

        #region Constructors
        public ProfielController(
            UserManager<ApplicationUser> userManager,
            IJobcoachRepository gebruikerRepository,
            SignInManager<ApplicationUser> signInManager,
            IExceptionLogRepository exceptionLogRepository,
            IOrganisatieRepository organisatieRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _gebruikerRepository = gebruikerRepository;
            _exceptionLogRepository = exceptionLogRepository;
            _organisatieRepository = organisatieRepository;
        }
        #endregion

        #region Index
        public IActionResult Index()
        {
            try
            {
                Jobcoach gebruiker = _gebruikerRepository.GetByEmail(HttpContext.User.Identity.Name);

                return View(new ProfielViewModel(gebruiker));
            }
            catch (Exception e)
            {
                _exceptionLogRepository.Add(new ExceptionLog(e, "Profiel", "Index"));
                _exceptionLogRepository.Save();
                TempData["error"] = "Er ging onverwachts iets fout tijdens het ophalen van je gegevens," +
                                    " probeer later opnieuw";
            }

            return RedirectToAction("Index", "Kairos");
        }
        #endregion

        #region Opslaan
        [HttpPost]
        public async Task<IActionResult> Opslaan(ProfielViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Jobcoach jobcoach = _gebruikerRepository.GetById(model.GebruikerId);

                    jobcoach.Voornaam = model.Voornaam;
                    jobcoach.Naam = model.Naam;
                    jobcoach.Emailadres = model.Email;

                    Organisatie organisatie = _organisatieRepository.GetById(model.OrganisatieId);

                    Organisatie nieuwe = new Organisatie(model.OrganisatieNaam, model.StraatOrganisatie, model.NrOrganisatie ?? 0,
                        model.BusOrganisatie, model.Postcode, model.Gemeente);

                    if (organisatie != null && !organisatie.Equals(nieuwe))
                    {
                        // ze zijn niet meer gelijk, dus de jobcoach heeft mogelijk iets gewijzigd dat bij de andere
                        // niet mag wijzigen
                        // of de jobcoach is van organisatie veranderd
                        jobcoach.Organisatie = nieuwe;
                    }
                    else
                    {
                        jobcoach.Organisatie.Naam = model.OrganisatieNaam;
                        jobcoach.Organisatie.Gemeente = model.Gemeente;

                        if (model.NrOrganisatie != null)
                            jobcoach.Organisatie.Nummer = (int)model.NrOrganisatie;

                        jobcoach.Organisatie.Postcode = model.Postcode;

                        jobcoach.Organisatie.Straat = model.StraatOrganisatie;
                        jobcoach.Organisatie.Bus = model.BusOrganisatie;
                    }
                    
                    if (model.Email != null && !model.Email.Equals(HttpContext.User.Identity.Name))
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
                    }

                    _gebruikerRepository.Save();

                    TempData["message"] = "Uw profiel is succesvol gewijzigd.";

                    return RedirectToAction(nameof(Index));
                }
                catch (Exception e)
                {
                    _exceptionLogRepository.Add(new ExceptionLog(e, "Profiel", "Opslaan"));
                    _exceptionLogRepository.Save();
                    TempData["error"] = "Er ging onverwachts iets mis, probeer later opnieuw";
                }
            }

            return RedirectToAction("Index");
        }
        #endregion
    }
}
