using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class WerkgeverViewModel
    {
        [HiddenInput]
        public int WerkgeverId { get; set; }

        [Display(Name = "Naam")]
        [Required(ErrorMessage = "Gelieve de naam van de organisatie op te geven")]
        public string Naam { get; set; }

        [Display(Name = "Straat")]
        public string Straat { get; set; } // niet verplicht: zie UC + backlog

        [Display(Name = "Nummer")]
        [Range(0, int.MaxValue, ErrorMessage = "Gelieve een positief getal op te geven voor het nummer")]
        public int Nummer{ get; set; } // niet verplicht: zie UC + backlog

        [Display(Name = "Postcode")]
        [DataType(DataType.PostalCode)]
        [Required(ErrorMessage = "Gelieve de postcode van de organisatie op te geven")]
        [Range(1000, 9999, ErrorMessage = "De postcode moet liggen tussen 1000 en 9999 (grenzen inbegrepen)")]
        public int Postcode { get; set; }

        [Display(Name = "Gemeente")]
        [Required(ErrorMessage = "Gelieve de gemeente van de organisatie op te geven")]
        public string Gemeente { get; set; }

        [Display(Name = "Gemiddeld aantal uren per week")]
        [Required(ErrorMessage = "Gelieve een gemiddeld aantal werkuren per week op te geven")]
        public double GemiddeldAantalWerkUrenPerWeek { get; set; }

        [Display(Name = "Patronale bijdrage")]
        [Required(ErrorMessage = "Gelieve de patronale bijdrage in te vullen")]
        public double PatronaleBijdrage { get; set; }

        public WerkgeverViewModel()
        {

        }

        public WerkgeverViewModel(Werkgever werkgever)
        {
            WerkgeverId = werkgever.WerkgeverId;
            Naam = werkgever.Naam;
            Straat = werkgever.Straat;
            Nummer = werkgever.Nummer;
            Postcode = werkgever.Postcode;
            Gemeente = werkgever.Gemeente;

            GemiddeldAantalWerkUrenPerWeek = Werkgever.AantalWerkuren;
            PatronaleBijdrage = Werkgever.PatronaleBijdrage;  
        }
    }
}
