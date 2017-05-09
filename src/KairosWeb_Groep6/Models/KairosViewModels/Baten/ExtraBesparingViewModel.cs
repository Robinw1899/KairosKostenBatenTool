using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten
{
    public class ExtraBesparingViewModel
    {
        #region Properties
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Required(ErrorMessage = "Gelieve een beschrijving op te geven.")]
        public string Beschrijving { get; set; }

        [Required(ErrorMessage = "Gelieve het bedrag op te geven.")]    
        public string Bedrag { get; set; }
        #endregion

        #region Constructors
        public ExtraBesparingViewModel()
        {
            
        }

        public ExtraBesparingViewModel(ExtraBesparing besparing)
        {
            DecimalConverter dc = new DecimalConverter();
            Id = besparing.Id;
            Type = besparing.Type;
            Soort = besparing.Soort;
            Beschrijving = besparing.Beschrijving;
            Bedrag = dc.ConvertToString(besparing.Bedrag);
        }
        #endregion
    }
}
