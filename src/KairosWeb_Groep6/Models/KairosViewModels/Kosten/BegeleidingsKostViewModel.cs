using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten
{
    public class BegeleidingsKostViewModel
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
        [Display(Name = "Bruto maandloon begeleider")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het bruto maandloon op te geven.")]
        public string BrutoMaandloonBegeleider { get; set; }

        [Display(Name = "Jaarbedrag")]
        public string Bedrag { get; set; } // jaarbedrag van de huidige BegeleidingsKost
        #endregion

        #region Constructors
        public BegeleidingsKostViewModel()
        {

        }
        public BegeleidingsKostViewModel(BegeleidingsKost kost)
        {
            DecimalConverter dc = new DecimalConverter();
            Id = kost.Id;
            Type = kost.Type;
            Soort = kost.Soort;
            Uren = kost.Uren;
            BrutoMaandloonBegeleider = dc.ConvertToString(kost.BrutoMaandloonBegeleider);
        }
        #endregion
    }
}
