using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.ProfielViewModels
{
    public class ProfielViewModel
    {
        [HiddenInput]
        public int GebruikerId { get; set; }

        [HiddenInput]
        public int OrganisatieId { get; set; }

        [Required(ErrorMessage = "Gelieve je naam in te vullen")]
        [Display(Name = "Naam", Prompt = "Naam")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Gelieve je voornaam in te vullen")]
        [Display(Name = "Voornaam", Prompt = "Voornaam")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Gelieve je emailadres in te vullen")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Een e-mail moet een '@' bevatten en moet eindigen op iets zoals '.be' of '.com'")]
        [Display(Name = "E-mailadres", Prompt = "E-mailades")]      
        public string Email { get; set; }

        /*info organisatie*/
        [Required(ErrorMessage = "Gelieve de naam van je organisatie in te vullen")]
        [Display(Name = "Naam organisatie", Prompt = "Naam organisatie")]
        public string OrganisatieNaam { get; set; }

        [Required(ErrorMessage = "Gelieve de straat van je organisatie in te vullen")]
        [Display(Name = "Straat", Prompt = "Straat")]
        public string StraatOrganisatie { get; set; }

        [Required(ErrorMessage = "Gelieve het huisnummer van je organisatie in te vullen")]
        [Display(Name = "Nr", Prompt = "Nr")]
        public int? NrOrganisatie { get; set; }

        [Display(Name = "Bus", Prompt = "Bus")]
        public string BusOrganisatie { get; set; }

        [Required(ErrorMessage = "Gelieve de postcode van je organisatie in te vullen")]
        [Display(Name = "Postcode", Prompt = "Postcode")]
        [DataType(DataType.PostalCode)]
        [Range(1000, 9999)]
        public int Postcode { get; set; }

        [Required(ErrorMessage = "Gelieve de gemeente van je organisatie in te vullen")]
        [Display(Name = "Gemeente", Prompt = "Gemeente")]
        public string Gemeente { get; set; }

        public ProfielViewModel()
        {

        }

        public ProfielViewModel(Jobcoach gebruiker)
        {
            GebruikerId = gebruiker.PersoonId;
            Naam = gebruiker.Naam;
            Voornaam = gebruiker.Voornaam;
            Email = gebruiker.Emailadres;

            if (gebruiker.Organisatie != null)
            {
                OrganisatieId = gebruiker.Organisatie.OrganisatieId;
                OrganisatieNaam = gebruiker.Organisatie.Naam;
                StraatOrganisatie = gebruiker.Organisatie.Straat;
                NrOrganisatie = gebruiker.Organisatie.Nummer;
                BusOrganisatie = gebruiker.Organisatie.Bus;
                Postcode = gebruiker.Organisatie.Postcode;
                Gemeente = gebruiker.Organisatie.Gemeente;
            }
        }
    }
}
