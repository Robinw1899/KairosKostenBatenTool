using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten
{
    public class MedewerkerNiveauBaatViewModel
    {
        #region Properties
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }
        
        [Required(ErrorMessage = "Gelieve een aantal uren op te geven")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het aantal uur op te geven.")]
        public decimal Uren { get; set; }

        [Required(ErrorMessage = "Gelieve een bruto maandloon op te geven")]
        [Display(Name = "Bruto maandloon (fulltime)")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het bruto maandloon op te geven.")]
        public decimal BrutoMaandloonFulltime { get; set; }

        public decimal Bedrag { get; set; }
        #endregion

        #region Constructors
        public MedewerkerNiveauBaatViewModel()
        {
            
        }

        public MedewerkerNiveauBaatViewModel(MedewerkerNiveauBaat baat)
        {
            Id = baat.Id;
            Type = baat.Type;
            Soort = baat.Soort;
            Uren = baat.Uren;
            BrutoMaandloonFulltime = baat.BrutoMaandloonFulltime;
            Bedrag = baat.Bedrag;
        }
        #endregion
    }
}
