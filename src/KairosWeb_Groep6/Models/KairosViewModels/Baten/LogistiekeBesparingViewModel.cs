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
        public string TransportKosten { get; set; }

        [Display(Name = "Jaarbedrag logistieke handlingskosten", Prompt = "Logistieke handlingskosten")]
        [Required(ErrorMessage = "Gelieve het jaarbedrag van de logistieke handlingskosten op te geven.")]
        public string LogistiekHandlingsKosten { get; set; }
        #endregion

        #region Constructors
        public LogistiekeBesparingViewModel()
        {
            Type = Type.Baat;
            Soort = Soort.LogistiekeBesparing;
        }

        public LogistiekeBesparingViewModel(LogistiekeBesparing baat)
            : this()
        {
            if (baat != null)
            {
                DecimalConverter dc = new DecimalConverter();
                Id = baat.Id;
                TransportKosten = dc.ConvertToString(baat.TransportKosten);
                LogistiekHandlingsKosten = dc.ConvertToString(baat.LogistiekHandlingsKosten);
            }
        }
        #endregion
    }
}
