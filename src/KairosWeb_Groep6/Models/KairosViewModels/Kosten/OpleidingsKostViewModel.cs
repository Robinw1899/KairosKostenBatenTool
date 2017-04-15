using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten
{
    public class OpleidingsKostViewModel
    {
        #region Properties
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Display(Name = "Type")]
        [Required(ErrorMessage = "Gelieve een (korte) beschrijving op te geven")]
        public string Beschrijving { get; set; }
      
        [Required(ErrorMessage = "Gelieve een bedrag op te geven")]
        [Range(0, Double.MaxValue, ErrorMessage = "Gelieve enkel een positief getal op te geven voor het bedrag")]
        public decimal Bedrag { get; set; }
        #endregion

        #region Constructors
        public OpleidingsKostViewModel()
        {
            
        }

        public OpleidingsKostViewModel(OpleidingsKost kost)
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
