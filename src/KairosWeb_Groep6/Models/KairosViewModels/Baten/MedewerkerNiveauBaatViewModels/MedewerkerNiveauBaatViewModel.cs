using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten.MedewerkerNiveauBaatViewModels
{
    public class MedewerkerNiveauBaatViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        //public string Beschrijving { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het aantal uur op te geven.")]
        public double Uren { get; set; }

        [Display(Name = "Bruto maandloon (fulltime)")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het aantal uur op te geven.")]
        public double BrutoMaandloonFulltime { get; set; }

        public double Bedrag { get; set; }

        public MedewerkerNiveauBaatViewModel(KostOfBaat kostOfBaat)
        {
            MedewerkerNiveauBaat baat = (MedewerkerNiveauBaat)kostOfBaat;

            Id = baat.Id;
            Type = baat.Type;
            Soort = baat.Soort;
            //Beschrijving = baat.Beschrijving;
            Uren = baat.Uren;
            BrutoMaandloonFulltime = baat.BrutoMaandloonFulltime;
            Bedrag = baat.Bedrag;
        }
    }
}
