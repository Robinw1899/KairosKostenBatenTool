using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.ExtraKostViewModels
{
    public class ExtraKostenIndexViewModel
    {
        #region Properties
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }
        [Display(Name = "Type")]
        [Required(ErrorMessage = "Gelieve een beschrijving op te geven.")]
        public string Beschrijving { get; set; }

        [Required(ErrorMessage = "Gelieve het bedrag op te geven.")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het bedrag op te geven.")]
        public double Bedrag { get; set; }

        public int ToonFormulier { get; set; } = 0;

        public IEnumerable<ExtraKostViewModel> ViewModels { get; set; }
        #endregion

        #region Constructors

        public ExtraKostenIndexViewModel()
        {
            Type = Type.Kost;
            Soort = Soort.ExtraKost;
        }

        public ExtraKostenIndexViewModel(ExtraKost kost)
            : this()
        {
            Id = kost.Id;
            Beschrijving = kost.Beschrijving;
            Bedrag = kost.Bedrag;
        }
        #endregion
    }
}
