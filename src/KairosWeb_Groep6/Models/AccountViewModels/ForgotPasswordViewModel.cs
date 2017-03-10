using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "Gelieve je emailadres op te geven")]
        [EmailAddress(ErrorMessage = "Gelieve een geldig emailadres op te geven")]
        public string Email { get; set; }
    }
}
