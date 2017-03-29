using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten
{
    public class OverurenBesparingViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Required(ErrorMessage = "Gelieve het bedrag op te geven.")]
        [Display(Name = "Jaarbedrag")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het bedrag op te geven.")]
        public double Bedrag { get; set; }

        public OverurenBesparingViewModel()
        {
            
        }

        public OverurenBesparingViewModel(OverurenBesparing besparing)
        {
            Id = besparing.Id;
            Type = besparing.Type;
            Soort = besparing.Soort;
            Bedrag = besparing.Bedrag;
        }
    }
}
