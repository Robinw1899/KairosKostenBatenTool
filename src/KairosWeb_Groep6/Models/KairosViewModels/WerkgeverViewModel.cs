using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class WerkgeverViewModel
    {
        #region Properties
        [HiddenInput]
        public int WerkgeverId { get; set; }

        [HiddenInput]
        public int DepartementId { get; set; }

        [Display(Name = "Departement", Prompt = "Departement")]
        [Required(ErrorMessage = "Gelieve het departement op te geven")]
        public string Departement { get; set; }

        [Display(Name = "Naam", Prompt = "Naam werkgever")]
        [Required(ErrorMessage = "Gelieve de naam van de werkgever op te geven")]
        public string Naam { get; set; }

        [Display(Name = "Straat", Prompt = "Straat")]
        public string Straat { get; set; } // niet verplicht: zie UC + backlog

        [Display(Name = "Nr", Prompt = "Nr")]
        [Range(0, int.MaxValue, ErrorMessage = "Gelieve een positief getal op te geven voor het nummer")]
        public int? Nummer{ get; set; } // niet verplicht: zie UC + backlog

        [Display(Prompt = "Bus")]
        public string Bus { get; set; } // niet verplicht: zie UC + backlog

        [Display(Name = "Postcode", Prompt = "Postcode")]
        [DataType(DataType.PostalCode)]
        [Required(ErrorMessage = "Gelieve de postcode van de organisatie op te geven")]
        [Range(1000, 9999, ErrorMessage = "De postcode moet liggen tussen 1000 en 9999 (grenzen inbegrepen)")]
        public int Postcode { get; set; }

        [Display(Name = "Gemeente", Prompt = "Gemeente")]
        [Required(ErrorMessage = "Gelieve de gemeente van de organisatie op te geven")]
        public string Gemeente { get; set; }

        [Display(Name = "Gemiddeld aantal werkuren per week", Prompt = "Gemiddeld aantal werkuren per week")]
        [Required(ErrorMessage = "Gelieve een gemiddeld aantal werkuren per week op te geven")]
        public string AantalWerkuren { get; set; }

        [Display(Name = "Patronale bijdrage", Prompt = "Patronale bijdrage (standaard 35%)")]   
        public string PatronaleBijdrage { get; set; }
        #endregion

        #region Constructors
        public WerkgeverViewModel()
        {

        }

        public WerkgeverViewModel(Departement departement)
            : this(departement.Werkgever)
        {
            DepartementId = departement.DepartementId;
            Departement = departement.Naam;
        }

        public WerkgeverViewModel(Werkgever werkgever)
        {
            if (werkgever != null)
            {
                DecimalConverter dc = new DecimalConverter();
                WerkgeverId = werkgever.WerkgeverId;
                Naam = werkgever.Naam;
                Straat = werkgever.Straat;
                Nummer = werkgever.Nummer;
                Bus = werkgever.Bus;
                Postcode = werkgever.Postcode;
                Gemeente = werkgever.Gemeente;
                AantalWerkuren = dc.ConvertToString(werkgever.AantalWerkuren);
                PatronaleBijdrage = dc.ConvertToString(werkgever.PatronaleBijdrage);
            }
        }
        #endregion
    }
}
