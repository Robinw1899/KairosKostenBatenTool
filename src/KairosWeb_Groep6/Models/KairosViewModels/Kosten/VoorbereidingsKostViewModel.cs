﻿using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten
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
        public string Bedrag { get; set; }
        #endregion

        #region Constructors
        public VoorbereidingsKostViewModel()
        {
            
        }

        public VoorbereidingsKostViewModel(VoorbereidingsKost kost)
        {
            DecimalConverter dc = new DecimalConverter();
            Id = kost.Id;
            Type = kost.Type;
            Soort = kost.Soort;
            Beschrijving = kost.Beschrijving;
            Bedrag = dc.ConvertToString(kost.Bedrag);
        }
        #endregion
    }
}
