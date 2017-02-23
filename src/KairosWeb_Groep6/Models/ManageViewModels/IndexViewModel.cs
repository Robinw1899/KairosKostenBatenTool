using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.ManageViewModels
{
    public class IndexViewModel
    {
        /* public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }*/
        public bool HasPassword { get; set; }
      
        [HiddenInput]
        public int JobcoachId { get; set; }

        public string PathImage { get; set; }

        [Display(Name = "Naam")]
        public string Naam { get; set; }
        [Display(Name = "Voornaam")]
        public string Voornaam { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Emailadres")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}", ErrorMessage = "Email adres is niet gelding")]
        public string Email { get; set; }

        /*info organisatie*/
        [Display(Name = "Naam")]
        public string OrganisatieNaam { get; set; }
        [Required]
        [Display(Name = "Straat")]
        public string StraatOrganisatie { get; set; }
        [Required]
        [Display(Name = "Nr")]
        public int NrOrganisatie { get; set; }
        [Required]
        [Display(Name = "Postcode")]
        [DataType(DataType.PostalCode)]
        [Range(1000, 9999)]
        public int Postcode { get; set; }
        [Required]
        [Display(Name = "Gemeente")]
        public string Gemeente { get; set; }

        public IndexViewModel()
        {

        }

        public IndexViewModel(Jobcoach jobcoach, Organisatie organisatie)
        {
            Naam = jobcoach.Naam;
            Voornaam = jobcoach.Voornaam;
            Email = jobcoach.Emailadres;
            // PathImage = jobcoach.PathImage == String.Empty? "":jobcoach.PathImage

            OrganisatieNaam = organisatie.Naam;
            StraatOrganisatie = organisatie.Straat;
            NrOrganisatie = organisatie.Nummer;
            Postcode = organisatie.Postcode;
            Gemeente = organisatie.Gemeente;
        }
    }
}

