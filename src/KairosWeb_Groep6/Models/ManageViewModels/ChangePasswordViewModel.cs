using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.ManageViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Huidig wachtwoord")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(16, ErrorMessage = "Het wachtwoord {0} moet tussen {2} en {1} karakters lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nieuw wachtwoord")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig nieuw wachtwoord")]
        [Compare("NewPassword", ErrorMessage = "Het nieuwe wachtwoord en het bevestig wachtwoord komen niet overeen.")]
        public string ConfirmPassword { get; set; }
    }
}
