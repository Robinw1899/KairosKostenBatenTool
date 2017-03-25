using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Gelieve je emailadres op te geven")]
        [EmailAddress(ErrorMessage = "Een e-mail moet een '@' bevatten en moet eindigen op iets zoals '.be' of '.com'")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gelieve je wachtwoord op te geven")]
        [DataType(DataType.Password)]
        [Display(Name = "Wachtwoord")]
        public string Password { get; set; }

        [Display(Name = "Mij onthouden")]
        public bool RememberMe { get; set; }

        public bool EersteKeerAanmelden { get; set; }
    }
}
