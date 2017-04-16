using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten
{
    public class UitzendKrachtBesparingViewModel
    {
        #region Properties
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Required(ErrorMessage = "Gelieve een (korte) beschrijving op te geven.")]
        public string Beschrijving { get; set; }

        [Required(ErrorMessage = "Gelieve een bedrag op te geven.")]
        [Display(Name = "Jaarbedrag")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positieve waarde op te geven voor het bedrag.")]
        public decimal Bedrag { get; set; }
        #endregion

        #region Constructors        
        public UitzendKrachtBesparingViewModel()
        {
            
        }

        public UitzendKrachtBesparingViewModel(UitzendKrachtBesparing baat)
        {
            Id = baat.Id;
            Type = baat.Type;
            Soort = baat.Soort;
            Beschrijving = baat.Beschrijving;
            Bedrag = baat.Bedrag;
        }
        #endregion
    }
}
