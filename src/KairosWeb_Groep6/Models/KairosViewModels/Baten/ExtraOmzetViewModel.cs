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
        public string JaarbedragOmzetverlies { get; set; }

        [Required(ErrorMessage = "Gelieve een percentage voor de besparing op te geven.")]
        [Display(Name = "% besparing")]      
        public string Besparing { get; set; }

        public string Bedrag { get; set; }
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
            DecimalConverter dc = new DecimalConverter();
            Id = omzet.Id;
            JaarbedragOmzetverlies = dc.ConvertToString(omzet.JaarbedragOmzetverlies);
            Besparing = dc.ConvertToString(omzet.Besparing);
            Bedrag = dc.ConvertToString(omzet.Bedrag);
        }
        #endregion
    }
}