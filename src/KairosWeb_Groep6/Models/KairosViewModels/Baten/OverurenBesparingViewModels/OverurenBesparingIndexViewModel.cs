using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten.OverurenBesparingViewModels
{
    public class OverurenBesparingIndexViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Required(ErrorMessage = "Gelieve het bedrag op te geven.")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het bedrag op te geven.")]
        public double Bedrag { get; set; }

        public OverurenBesparingIndexViewModel()
        {
            
        }

        public OverurenBesparingIndexViewModel(OverurenBesparing besparing)
        {
            Id = besparing.Id;
            Type = besparing.Type;
            Soort = besparing.Soort;
            Bedrag = besparing.Bedrag;
        }
    }
}
