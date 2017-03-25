using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.EnclaveKostViewModels
{
    public class EnclaveKostenIndexViewModel
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
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve enkel een positief bedrag op te geven.")]
        public double Bedrag { get; set; }

        public IEnumerable<EnclaveKostenViewModel> ViewModels { get; set; }
        #endregion

        #region Constructors
        public EnclaveKostenIndexViewModel()
        {

        }

        public EnclaveKostenIndexViewModel(EnclaveKost kost)
        {
            Type = kost.Type;
            Soort = kost.Soort;
            Beschrijving = kost.Beschrijving;
            Bedrag = kost.Bedrag;
        }
        #endregion
    }
}
