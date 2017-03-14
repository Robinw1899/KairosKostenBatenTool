using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten.ExterneInkoopViewModels
{
    public class ExterneInkopenIndexViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        [HiddenInput]
        public Type Type { get; set; }
        [HiddenInput]
        public Soort Soort { get; set; }

        [Required(ErrorMessage = "Gelieve een (korte) beschrijving op te geven")]
        public string Beschrijving { get; set; }

        [Required(ErrorMessage = "Gelieve een bedrag op te geven")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Gelieve een positief bedrag op te geven")]
        public double Bedrag { get; set; }

        public IEnumerable<ExterneInkoopViewModel> ViewModels { get; set; }

        public ExterneInkopenIndexViewModel()
        {
            
        }

        public ExterneInkopenIndexViewModel(ExterneInkoop inkoop)
        {
            Id = inkoop.Id;
            Type = inkoop.Type;
            Soort = inkoop.Soort;
            Beschrijving = inkoop.Beschrijving;
            Bedrag = inkoop.Bedrag;
        }
    }
}
