﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.AccountViewModels;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Services;

namespace KairosWeb_Groep6.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly IJobcoachRepository _jobcoachRepository;
        private readonly IIntroductietekstRepository _introductietekstRepository;
        private readonly IOrganisatieRepository _organisatieRepository;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory,
            IJobcoachRepository jobcoachRepository,
            IIntroductietekstRepository introductietekstRepository,
            IOrganisatieRepository organisatieRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _jobcoachRepository = jobcoachRepository;
            _introductietekstRepository = introductietekstRepository;
            _organisatieRepository = organisatieRepository;
        }

        //
        // GET: /Account/Login
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            TempData["Actie"] = "Aanmelden"; // nodig om de partials niet te laden
            Introductietekst tekst = _introductietekstRepository.GetIntroductietekst();

            LoginViewModel model = new LoginViewModel
            {
                Introductietekst = tekst
            };

            return View(model);
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    Jobcoach gebruiker = _jobcoachRepository.GetByEmail(model.Email);

                    if (gebruiker != null)
                    {
                        if (!gebruiker.AlAangemeld)
                        {
                            return RedirectToAction(nameof(KairosController.EersteKeerAanmelden), "Kairos");
                        }
                    }
                    return RedirectToLocal(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    return View("Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Deze combinatie email en wachtwoord is ongeldig, probeer opnieuw.");
                    TempData["Actie"] = "Aanmelden"; // nodig om de partials niet te laden
                    Introductietekst tekst = _introductietekstRepository.GetIntroductietekst();

                    model.Introductietekst = tekst;

                    return View(model);
                }
            }
            
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError(string.Empty, "Iets ging fout, probeer later opnieuw.");
            return View(model);
        }

        //
        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            TempData["Actie"] = "Registreer";
            return View();
        }

       

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            Random random = new Random();
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Naam = model.Naam,
                    Voornaam = model.Voornaam
                };
                
                var password = PasswordGenerator.GeneratePassword(random.Next(6, 16));
                //var password = "kairos2017";

                var possibleUser = await _userManager.FindByEmailAsync(model.Email);

                if (possibleUser != null)
                {
                    ModelState.AddModelError("", "Er is al een gebruiker geregistreerd met dit emailadres");
                    TempData["Actie"] = "Registreer";
                }
                else
                {
                    Organisatie organisatie = new Organisatie(model.OrganisatieNaam, model.StraatOrganisatie,
                            model.NrOrganisatie, model.Bus, model.Postcode, model.Gemeente);

                    IEnumerable<Organisatie> possibleOrgs = _organisatieRepository.GetAllByNaam(model.OrganisatieNaam);

                    if (possibleOrgs.Any())
                    {// er kan een dubbele organisatie inkomen als we nu niet ingrijpen, we zoeken de evt dubbele
                        Organisatie dubbel = possibleOrgs.SingleOrDefault(o => o.Equals(organisatie));

                        if (dubbel != null)
                        {
                            organisatie = dubbel;
                        }
                    }

                    Jobcoach jobcoach = new Jobcoach(model.Naam, model.Voornaam, model.Email, organisatie);
                    _jobcoachRepository.Add(jobcoach);
                    _jobcoachRepository.Save();

                    string naam = model.Voornaam ?? "";
                    bool mailVerzendenGelukt = await EmailSender.SendRegisterMailWithPassword(naam, model.Email, password);

                    if (mailVerzendenGelukt)
                    {
                        var result = await _userManager.CreateAsync(user, password);

                        if (result.Succeeded)
                        {
                            // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                            // Send an email with this link
                            //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                            //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                            //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                            //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                            //await _signInManager.SignInAsync(user, isPersistent: false);
                            //_logger.LogInformation(3, "User created a new account with password.");
                            //return RedirectToLocal(returnUrl);

                            TempData["message"] =
                                "Je bent succesvol geregistreerd. Kijk snel je mail na voor je tijdelijk wachtwoord!";

                            return RedirectToAction(nameof(Login), "Account");
                        }

                        TempData["Actie"] = "Registreer";
                        AddErrors(result);
                    }
                    else
                    {
                        TempData["Actie"] = "Registreer";
                        TempData["error"] = "Registreren is niet mogelijk op dit moment, probeer het later opnieuw.";
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            TempData["message"] = "Je bent succesvol afgemeld!";
            return RedirectToAction(nameof(KairosController.Index), "Kairos");
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // Request a redirect to the external login provider.
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        //
        // GET: /Account/ExternalLoginCallback
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return View(nameof(Login));
            }
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction(nameof(Login));
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            if (result.Succeeded)
            {
                _logger.LogInformation(5, "User logged in with {Name} provider.", info.LoginProvider);
                return RedirectToLocal(returnUrl);
            }
            if (result.RequiresTwoFactor)
            {
                return RedirectToAction(nameof(SendCode), new { ReturnUrl = returnUrl });
            }
            if (result.IsLockedOut)
            {
                return View("Lockout");
            }
            else
            {
                // If the user does not have an account, then ask the user to create an account.
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["LoginProvider"] = info.LoginProvider;
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        _logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        // GET: /Account/ConfirmEmail
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("Error");
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //
        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            TempData["Actie"] = "Registreer";
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    TempData["Actie"] = "Registreer";
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                //   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

                // eerst mail proberen verzenden alvorens paswoord te resetten
                //Random random = new Random();
                //var password = PasswordGenerator.GeneratePassword(random.Next(6, 16));
                var jobcoach = _jobcoachRepository.GetByEmail(user.Email);
                _jobcoachRepository.Save();

                string name = jobcoach.Voornaam ?? "";
                string url = Url.Action("ResetPassword", "Account", new { email = jobcoach.Emailadres}, protocol: HttpContext.Request.Scheme);

                bool mailVerzendenGelukt = await EmailSender.SendForgotPasswordMail(name, jobcoach.Emailadres, url);

                if (mailVerzendenGelukt)
                {
                    //var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    //await _userManager.ResetPasswordAsync(user, token, password);
                    TempData["Actie"] = "Registreer";

                    return View("ForgotPasswordConfirmation");
                }
                else
                {
                    TempData["Actie"] = "Registreer";
                    TempData["error"] = "Je wachtwoord resetten is niet mogelijk op dit moment, probeer het later opnieuw. Je wachtwoord is nog steeds hetzelfde als hiervoor.";
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string email)
        {
            TempData["Actie"] = "Registreer";
            ResetPasswordViewModel model = new ResetPasswordViewModel {Email = email};
            return View(model);
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            TempData["Actie"] = "Registreer";
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/SendCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl = null, bool rememberMe = false)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var userFactors = await _userManager.GetValidTwoFactorProvidersAsync(user);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            // Generate the token and send it
            var code = await _userManager.GenerateTwoFactorTokenAsync(user, model.SelectedProvider);
            if (string.IsNullOrWhiteSpace(code))
            {
                return View("Error");
            }

            var message = "Your security code is: " + code;
            if (model.SelectedProvider == "Email")
            {
                await _emailSender.SendEmailAsync(await _userManager.GetEmailAsync(user), "Security Code", message);
            }
            else if (model.SelectedProvider == "Phone")
            {
                await _smsSender.SendSmsAsync(await _userManager.GetPhoneNumberAsync(user), message);
            }

            return RedirectToAction(nameof(VerifyCode), new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/VerifyCode
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(string provider, bool rememberMe, string returnUrl = null)
        {
            // Require that the user has already logged in via username/password or external login
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes.
            // If a user enters incorrect codes for a specified amount of time then the user account
            // will be locked out for a specified amount of time.
            var result = await _signInManager.TwoFactorSignInAsync(model.Provider, model.Code, model.RememberMe, model.RememberBrowser);
            if (result.Succeeded)
            {
                return RedirectToLocal(model.ReturnUrl);
            }
            if (result.IsLockedOut)
            {
                _logger.LogWarning(7, "User account locked out.");
                return View("Lockout");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid code.");
                return View(model);
            }
        }

        #region Helpers

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private Task<ApplicationUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(HttpContext.User);
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(KairosController.Index), "Kairos");
            }
        }

        #endregion
    }
}
