using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.AccountViewModels
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
