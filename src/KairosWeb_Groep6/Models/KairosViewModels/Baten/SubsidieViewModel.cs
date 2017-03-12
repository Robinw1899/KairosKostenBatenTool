using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten
{
    public class SubsidieViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Display(Name = "Jaarbedrag")]
        [Required(ErrorMessage = "Gelieve het bedrag op te geven.")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het bedrag op te geven.")]
        public double Bedrag { get; set; }

        public SubsidieViewModel()
        {
            
        }

        public SubsidieViewModel(Subsidie subsidie)
        {
            Id = subsidie.Id;
            Type = subsidie.Type;
            Soort = subsidie.Soort;
            Bedrag = subsidie.Bedrag;
        }
    }
}
