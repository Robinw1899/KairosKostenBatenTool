using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class ContactPersoonViewModel
    {
        #region Properties
        [HiddenInput]
        public int PersoonId { get; set; }

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

        public int WerkgeverId { get; set; }

        
        #endregion

        #region Constructor
        public ContactPersoonViewModel()
        {

        }

        public ContactPersoonViewModel(ContactPersoon contactPersoon, int id)
        {
            PersoonId = contactPersoon.ContactPersoonId;

            Voornaam = contactPersoon.Voornaam;

            Naam = contactPersoon.Naam;

            Email = contactPersoon.Emailadres;

            WerkgeverId = id;

        }
       
        #endregion
    }
}
