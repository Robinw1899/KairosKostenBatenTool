﻿using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.InfrastructuurKostenViewModels
{
    public class InfrastructuurKostenViewModel
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
        #endregion

        #region Constructors
        public InfrastructuurKostenViewModel()
        {

        }

        public InfrastructuurKostenViewModel(InfrastructuurKost kost)
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
