using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class EersteKeerAanmeldenViewModel
    {
        [HiddenInput]
        public string Email { get; set; }

        [Required(ErrorMessage = "Gelieve je wachtwoord in te vullen")]
        [Display(Name = "Wachtwoord")]
        [StringLength(16, ErrorMessage = "Het {0} moet minstens {2} en mag maximum {1} karakters lang zijn.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Gelieve je wachtwoord te bevestigen")]
        [DataType(DataType.Password)]
        [Display(Name = "Bevestig wachtwoord")]
        [Compare("Password", ErrorMessage = "Het wachtwoord en bevestigingswachtwoord komen niet overeen.")]
        public string ConfirmPassword { get; set; }

        public bool AlAangemeld { get; set; }
    }
}
