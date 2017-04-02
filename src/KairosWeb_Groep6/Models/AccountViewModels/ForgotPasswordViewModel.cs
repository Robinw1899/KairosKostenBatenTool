using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Display(Name = "E-mailadres", Prompt = "E-mailadres")]
        [Required(ErrorMessage = "Gelieve je emailadres op te geven")]
        [EmailAddress(ErrorMessage = "Een e-mail moet een '@' bevatten en moet eindigen op iets zoals '.be' of '.com'")]
        public string Email { get; set; }
    }
}
