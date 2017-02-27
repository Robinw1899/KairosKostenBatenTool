using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.ProfielViewModels
{
    public class ProfielViewModel
    {
        [HiddenInput]
        public int GebruikerId { get; set; }

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
        public int? NrOrganisatie { get; set; }

        [Required]
        [Display(Name = "Postcode")]
        [DataType(DataType.PostalCode)]
        [Range(1000, 9999)]
        public int? Postcode { get; set; }

        [Required]
        [Display(Name = "Gemeente")]
        public string Gemeente { get; set; }

        public ProfielViewModel()
        {

        }

        public ProfielViewModel(Gebruiker gebruiker)
        {
            GebruikerId = gebruiker.GebruikerId;
            Naam = gebruiker.Naam;
            Voornaam = gebruiker.Voornaam;
            Email = gebruiker.Emailadres;
            // PathImage = jobcoach.PathImage == String.Empty? "":jobcoach.PathImage

            if (gebruiker.Organisatie != null)
            {
                OrganisatieNaam = gebruiker.Organisatie.Naam;
                StraatOrganisatie = gebruiker.Organisatie.Straat;
                NrOrganisatie = gebruiker.Organisatie.Nummer;
                Postcode = gebruiker.Organisatie.Postcode;
                Gemeente = gebruiker.Organisatie.Gemeente;
            }
        }
    }
}
