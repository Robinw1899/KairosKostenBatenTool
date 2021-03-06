﻿using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten
{
    public class ExtraKostViewModel
    {
        #region Properties
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Required(ErrorMessage = "Gelieve een beschrijving op te geven.")]
        [Display(Name = "Type")]
        public string Beschrijving { get; set; }

        [Required(ErrorMessage = "Gelieve het bedrag op te geven.")]      
        public string Bedrag { get; set; }
        #endregion

        #region Constructors

        public ExtraKostViewModel()
        {
            
        }

        public ExtraKostViewModel(ExtraKost kost)
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
