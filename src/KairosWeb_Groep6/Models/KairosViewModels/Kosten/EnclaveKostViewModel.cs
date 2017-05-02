using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten
{
    public class EnclaveKostViewModel
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
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve enkel een positief bedrag op te geven.")]
        public string Bedrag { get; set; }
        #endregion

        #region Constructors
        public EnclaveKostViewModel()
        {

        }
        public EnclaveKostViewModel(EnclaveKost kost)
        {
            DecimalConverter dc = new DecimalConverter();
            Id = kost.Id;
            Type = kost.Type;
            Soort = kost.Soort;
            Bedrag = dc.ConvertToString(kost.Bedrag);
            Beschrijving = kost.Beschrijving;
        }
        #endregion
    }
}
