﻿using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Emailadres is verplicht")]
        [EmailAddress]
        [Display(Name = "E-mailadres", Prompt = "E-mailadres")]
        public string Email { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Het {0} mag minstens {2} en maximuaal {1} tekens bevatten.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord", Prompt = "Wachtwoord")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord", Prompt = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "Het wachtwoord en bevestig wachtwoord komen niet overeen.")]
        public string ConfirmPassword { get; set; }
    }
}
