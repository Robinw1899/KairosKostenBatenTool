﻿using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten
{
    public class ExtraProductiviteitViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Display(Name = "Jaarbedrag")]
        [Required(ErrorMessage = "Gelieve het bedrag op te geven.")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het bedrag op te geven.")]
        public decimal Bedrag { get; set; }

        public ExtraProductiviteitViewModel()
        {
            Type = Type.Baat;
            Soort = Soort.ExtraProductiviteit;
        }

        public ExtraProductiviteitViewModel(ExtraProductiviteit productiviteit)
            : this()
        {
            Id = productiviteit.Id;
            Bedrag = productiviteit.Bedrag;
        }
    }
}
