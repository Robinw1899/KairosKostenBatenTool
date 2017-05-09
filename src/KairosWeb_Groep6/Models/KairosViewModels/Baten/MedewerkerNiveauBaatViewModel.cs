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
        public decimal Uren { get; set; }

        [Required(ErrorMessage = "Gelieve een bruto maandloon op te geven")]
        [Display(Name = "Bruto maandloon (fulltime)")]     
        public string BrutoMaandloonFulltime { get; set; }

        public string Bedrag { get; set; }
        #endregion

        #region Constructors
        public MedewerkerNiveauBaatViewModel()
        {
            
        }

        public MedewerkerNiveauBaatViewModel(MedewerkerNiveauBaat baat)
        {
            DecimalConverter dc = new DecimalConverter();
            Id = baat.Id;
            Type = baat.Type;
            Soort = baat.Soort;
            Uren = baat.Uren;
            BrutoMaandloonFulltime = dc.ConvertToString(baat.BrutoMaandloonFulltime);
            Bedrag = dc.ConvertToString(baat.Bedrag);
        }
        #endregion
    }
}
