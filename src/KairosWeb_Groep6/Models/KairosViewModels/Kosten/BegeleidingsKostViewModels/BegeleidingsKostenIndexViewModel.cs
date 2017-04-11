using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.BegeleidingsKostViewModels
{
    public class BegeleidingsKostenIndexViewModel
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
        public double Uren { get; set; }

        [Required(ErrorMessage = "Gelieve een bruto maandloon op te geven")]
        [Display(Name = "Bruto maandloon begeleider")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het bruto maandloon op te geven.")]
        public double BrutoMaandloonBegeleider { get; set; }
        [Display(Name ="Jaarbedrag")]
        public double Bedrag { get; set; } // jaarbedrag van de huidige BegeleidingsKost

        public int ToonFormulier { get; set; } = 0;

        public IEnumerable<BegeleidingsKostViewModel> ViewModels { get; set; }
        #endregion

        #region Constructors

        public BegeleidingsKostenIndexViewModel()
        {
            Type = Type.Kost;
            Soort = Soort.BegeleidingsKost;
        }
        #endregion
    }
}
