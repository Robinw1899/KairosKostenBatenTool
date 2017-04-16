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
        #region Properties
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Display(Name = "Jaarbedrag omzetverlies")]
        [Required(ErrorMessage = "Gelieve het jaarbedrag van de omzetverlies op te geven.")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positieve waarde op te geven voor het jaarbedrag.")]
        public decimal JaarbedragOmzetverlies { get; set; }

        [Required(ErrorMessage = "Gelieve een percentage voor de besparing op te geven.")]
        [Display(Name = "% besparing")]
        [Range(typeof(decimal), "0.00", "100.00", ErrorMessage = "Gelieve een geldig percentage tussen 0 en 100 op te geven.")]
        public decimal Besparing { get; set; }

        public decimal Bedrag { get; set; }
        #endregion

        #region Constructors       
        public ExtraOmzetViewModel()
        {
            Type = Type.Baat;
            Soort = Soort.ExtraOmzet;
        }

        public ExtraOmzetViewModel(ExtraOmzet omzet)
            : this()
        {
            Id = omzet.Id;
            JaarbedragOmzetverlies = omzet.JaarbedragOmzetverlies;
            Besparing = omzet.Besparing;
            Bedrag = omzet.Bedrag;
        }
        #endregion
    }
}