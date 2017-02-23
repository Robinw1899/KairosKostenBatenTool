using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class EersteKeerAanmeldenViewModel
    {
        public string Email { get; set; }

        [Required]
        [Display(Name = "Wachtwoord")]
        [StringLength(16, ErrorMessage = "Het wachtwoord {0} moet minstens {2} en mag maximum {1} karakters lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "Het wachtwoord en bevestigingswachtwoord komen niet overeen.")]
        public string ConfirmPassword { get; set; }
    }
}
