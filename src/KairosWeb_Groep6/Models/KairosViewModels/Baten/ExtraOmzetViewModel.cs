using System;
using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten
{
    public class ExtraOmzetViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Display(Name = "Jaarbedrag omzetverlies")]
        [Required(ErrorMessage = "Gelieve het jaarbedrag van de omzetverlies op te geven.")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positieve waarde op te geven voor het jaarbedrag.")]
        public double JaarbedragOmzetverlies { get; set; }

        [Required(ErrorMessage = "Gelieve een percentage voor de besparing op te geven.")]
        [Display(Name = "% besparing")]
        [Range(typeof(double), "0", "100", ErrorMessage = "Gelieve een geldig percentage tussen 0 en 100 op te geven.")]
        public double Besparing { get; set; }

        public double Bedrag { get; set; }

        public ExtraOmzetViewModel()
        {
        }

        public ExtraOmzetViewModel(ExtraOmzet omzet)
        {
            Id = omzet.Id;
            Type = omzet.Type;
            Soort = omzet.Soort;
            JaarbedragOmzetverlies = omzet.JaarbedragOmzetverlies;
            Besparing = omzet.Besparing;
            Bedrag = omzet.Bedrag;
        }
    }
}