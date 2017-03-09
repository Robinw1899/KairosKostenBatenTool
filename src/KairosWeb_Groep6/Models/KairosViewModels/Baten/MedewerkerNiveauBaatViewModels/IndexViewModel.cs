using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten.MedewerkerNiveauBaatViewModels
{
    public class IndexViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het aantal uur op te geven.")]
        public double Uren { get; set; }

        [Display(Name = "Bruto maandloon (fulltime)")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve een positief getal voor het aantal uur op te geven.")]
        public double BrutoMaandloonFulltime { get; set; }

        public IEnumerable<MedewerkerNiveauBaatViewModel> ViewModels { get; set; }
    }
}
