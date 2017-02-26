﻿using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [HiddenInput]
        public int JobcoachId { get; set; }

        [Required(ErrorMessage = "Naam is verplicht")]
        [Display(Name = "Naam")]
        public string Naam { get; set; }

        [Required(ErrorMessage = "Voornaam is verplicht")]
        [Display(Name = "Voornaam")]
        public string Voornaam { get; set; }

        [Required(ErrorMessage = "Emailadres is verplicht")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Emailadres")]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}", ErrorMessage = "Email adres is niet gelding")]
        public string Email { get; set; }

        /*info organisatie*/
        [Required(ErrorMessage = "De naam van de organisatie is verplicht")]
        [Display(Name = "Naam")]
        public string OrganisatieNaam { get; set; }

        [Required(ErrorMessage = "De straat van de organisatie is verplicht")]
        [Display(Name = "Straat")]
        public string StraatOrganisatie { get; set; }

        [Required(ErrorMessage = "Het huisnummer van de organisatie is verplicht")]
        [Display(Name = "Nr")]
        public int NrOrganisatie { get; set; }

        [Required(ErrorMessage = "De postcode van de organisatie is verplicht")]
        [Display(Name = "Postcode")]
        [DataType(DataType.PostalCode)]
        [Range(1000, 9999, ErrorMessage = "De postcode moet liggen tussen 1000 en 9999 (grenzen inbegrepen)")]
        public int Postcode { get; set; }

        [Required(ErrorMessage = "De gemeente van de organisatie is verplicht")]
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
    }

}

