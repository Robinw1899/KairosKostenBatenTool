using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten
{
    public class LogistiekeBesparingViewModel
    {
        #region Properties
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Display(Name = "Jaarbedrag transportkosten", Prompt = "Transportkosten")]
        [Required(ErrorMessage = "Gelieve het jaarbedrag van de transportkosten op te geven.")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het jaarbedrag van de transportkosten op te geven.")]
        public double TransportKosten { get; set; }

        [Display(Name = "Jaarbedrag logistieke handlingskosten", Prompt = "Logistieke handlingskosten")]
        [Required(ErrorMessage = "Gelieve het jaarbedrag van de logistieke handlingskosten op te geven.")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het jaarbedrag van de logistieke handlingskosten op te geven.")]
        public double LogistiekHandlingsKosten { get; set; }

        public bool ToonFormulier { get; set; }
        #endregion

        #region Constructors
        public LogistiekeBesparingViewModel()
        {
            
        }

        public LogistiekeBesparingViewModel(LogistiekeBesparing baat)
        {
            if (baat != null)
            {
                Id = baat.Id;
                Type = baat.Type;
                Soort = baat.Soort;
                TransportKosten = baat.TransportKosten;
                LogistiekHandlingsKosten = baat.LogistiekHandlingsKosten;
            }
        }
        #endregion
    }
}
