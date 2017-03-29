﻿using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.InfrastructuurKostenViewModels
{
    public class InfrastructuurKostenIndexViewModel
    {

        #region Properties
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Domain.Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }
        [Display(Name = "Type")]
        [Required(ErrorMessage = "Gelieve een (korte) beschrijving op te geven.")]
        public string Beschrijving { get; set; }

        [Required(ErrorMessage = "Gelieve een bedrag op te geven.")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve enkel een positief getal in te geven voor het bedrag")]

        public double Bedrag { get; set; }

        public int ToonFormulier { get; set; } = 0;

        public IEnumerable<InfrastructuurKostenViewModel> ViewModels { get; set; } 
        #endregion
    }
}
