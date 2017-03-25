using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.VoorbereidingsKostViewModels
{
    public class VoorbereidingsKostViewModel
    {
        #region Properties
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Required(ErrorMessage = "Gelieve een type op te geven")]
        [Display(Name = "Type")]
        public string Beschrijving { get; set; }

        [Required(ErrorMessage = "Gelieve een bedrag in te vullen")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve enkel een positief getal in te vullen voor het bedrag")]
        public double Bedrag { get; set; }
        #endregion

        #region Constructors
        public VoorbereidingsKostViewModel()
        {
            
        }

        public VoorbereidingsKostViewModel(VoorbereidingsKost kost)
        {
            Id = kost.Id;
            Type = kost.Type;
            Soort = kost.Soort;
            Beschrijving = kost.Beschrijving;
            Bedrag = kost.Bedrag;
        }
        #endregion
    }
}
