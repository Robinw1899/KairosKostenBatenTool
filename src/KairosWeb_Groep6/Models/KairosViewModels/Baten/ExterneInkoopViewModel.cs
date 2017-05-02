using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten
{
    public class ExterneInkoopViewModel
    {
        #region Properties
        [HiddenInput]
        public int Id { get; set; }
        [HiddenInput]
        public Type Type { get; set; }
        [HiddenInput]
        public Soort Soort { get; set; }
        
        [Required(ErrorMessage = "Gelieve een beschrijving in te vullen")]
        public string Beschrijving { get; set; }

        [Required(ErrorMessage = "Gelieve een bedrag in te vullen")]
        [Display(Name = "Jaarbedrag")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het aantal uur op te geven.")]        
        public string Bedrag { get; set; }
        #endregion

        #region Constructors
        public ExterneInkoopViewModel()
        {
            
        }

        public ExterneInkoopViewModel(ExterneInkoop baat)
        {
            DecimalConverter dc = new DecimalConverter();
            Id = baat.Id;
            Type = baat.Type;
            Soort = baat.Soort;
            Beschrijving = baat.Beschrijving;
            Bedrag = dc.ConvertToString(baat.Bedrag);
        }
        #endregion
    }
}
