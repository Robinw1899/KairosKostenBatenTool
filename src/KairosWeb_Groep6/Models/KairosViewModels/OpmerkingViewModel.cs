using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class OpmerkingViewModel
    {
        [Required]
        [DataType(DataType.Text)]
        public string Onderwerp { get; set; }
        [Required]
        [DataType(DataType.Text)]
        public string Bericht { get; set; }
        [HiddenInput]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [HiddenInput]
        public string Voornaam { get; set; }
        [HiddenInput]
        public string Naam { get; set; }

        public OpmerkingViewModel(Jobcoach jobCoach, string onderwerp, string bericht)
        {
            Email = jobCoach.Emailadres;
            Voornaam = jobCoach.Voornaam;
            Naam = jobCoach.Naam;
            Onderwerp = onderwerp;
            Bericht = bericht;
        }
    }
}
