using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.ProfielViewModels
{
    public class ProfielViewModel
    {
        [HiddenInput]
        public int GebruikerId { get; set; }

        //public string PathImage { get; set; }

        [Required(ErrorMessage = "Gelieve je naam in te vullen")]
        [Display(Name = "Naam")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Gelieve je voornaam in te vullen")]
        [Display(Name = "Voornaam")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Gelieve je emailadres in te vullen")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Een e-mail moet een '@' bevatten en moet eindigen op iets zoals '.be' of '.com'")]
        [Display(Name = "Emailadres")]      
        public string Email { get; set; }

        /*info organisatie*/
        [Required(ErrorMessage = "Gelieve de naam van je organisatie in te vullen")]
        [Display(Name = "Naam")]
        public string OrganisatieNaam { get; set; }

        [Required(ErrorMessage = "Gelieve de straat van je organisatie in te vullen")]
        [Display(Name = "Straat")]
        public string StraatOrganisatie { get; set; }

        [Required(ErrorMessage = "Gelieve de huisnummer van je organisatie in te vullen")]
        [Display(Name = "Nr")]
        public int? NrOrganisatie { get; set; }

        [Required(ErrorMessage = "Gelieve de postcode van je organisatie in te vullen")]
        [Display(Name = "Postcode")]
        [DataType(DataType.PostalCode)]
        [Range(1000, 9999)]
        public int? Postcode { get; set; }

        [Required(ErrorMessage = "Gelieve de gemeente van je organisatie in te vullen")]
        [Display(Name = "Gemeente")]
        public string Gemeente { get; set; }

        public ProfielViewModel()
        {

        }

        public ProfielViewModel(Jobcoach gebruiker)
        {
            GebruikerId = gebruiker.JobcoachId;
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
