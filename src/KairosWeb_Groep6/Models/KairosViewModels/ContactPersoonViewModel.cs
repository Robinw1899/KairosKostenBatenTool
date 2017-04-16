using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class ContactPersoonViewModel
    {
        #region Properties
        [HiddenInput]
        public int PersoonId { get; set; }

        [HiddenInput]
        public int WerkgeverId { get; set; }

        [Display(Name = "Naam", Prompt = "Naam contactPersoon")]
        [Required(ErrorMessage = "Gelieve het contactpersoon op te geven")]
        public string Naam { get; set; }

        [Display(Name = "Voornaam", Prompt = "Voornaam contactpersoon")]
        [Required(ErrorMessage = "Gelieve de naam van de contactpersoon op te geven")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Gelieve je emailadres in te vullen")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Een e-mail moet een '@' bevatten en moet eindigen op iets zoals '.be' of '.com'")]
        [Display(Name = "E-mailadres", Prompt = "E-mailades")]
        public string Email { get; set; }

        [Display(Name = "Is dit de hoofdcontactpersoon?")]
        public bool IsHoofd { get; set; }
        #endregion

        #region Constructor
        public ContactPersoonViewModel()
        {

        }

        public ContactPersoonViewModel(ContactPersoon contactPersoon, int werkgeverid)
        {
            PersoonId = contactPersoon.ContactPersoonId;

            Voornaam = contactPersoon.Voornaam;

            Naam = contactPersoon.Naam;

            Email = contactPersoon.Emailadres;

            IsHoofd = contactPersoon.IsHoofdContactPersoon;

            WerkgeverId = werkgeverid;
        }
        #endregion
    }
}
