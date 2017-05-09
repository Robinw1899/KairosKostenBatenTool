using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten
{
    public class SubsidieViewModel
    {
        #region Properties
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Display(Name = "Jaarbedrag")]
        [Required(ErrorMessage = "Gelieve het bedrag op te geven.")]      
        public string Bedrag { get; set; }
        #endregion

        #region Constructors        
        public SubsidieViewModel()
        {
            Type = Type.Baat;
            Soort = Soort.Subsidie;
        }

        public SubsidieViewModel(Subsidie subsidie)
            : this()
        {
            DecimalConverter dc = new DecimalConverter();
            Id = subsidie.Id;
            Bedrag = dc.ConvertToString(subsidie.Bedrag);
        }
        #endregion
    }
}
