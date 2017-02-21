using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.AccountViewModels
{
    public class RegisterViewModel
    {
/*info jobcoach*/

        [HiddenInput]
        public int JobcoachId { get; set; }
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

        public RegisterViewModel()
        {
            
        }

        public RegisterViewModel(Jobcoach jobcoach, Organisatie organisatie)
        {
            Naam = jobcoach.Naam;
            Voornaam = jobcoach.Voornaam;
            Email = jobcoach.Emailadres;

            OrganisatieNaam = organisatie.Naam;
            StraatOrganisatie = organisatie.Straat;
            NrOrganisatie = organisatie.Nummer;
            Postcode = organisatie.Postcode;
            Gemeente = organisatie.Gemeente;
        }
        /*
                [Required]
                [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
                [DataType(DataType.Password)]
                [Display(Name = "Password")]
                public string Password { get; set; }

                [DataType(DataType.Password)]
                [Display(Name = "Confirm password")]
                [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
                public string ConfirmPassword { get; set; }
        */
    }

}

