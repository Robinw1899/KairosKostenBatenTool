using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

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
        [Display(Name = "Jaarbedrag")]
        [Range(0.0, double.MaxValue, ErrorMessage = "Gelieve een positief bedrag op te geven")]
        public double Bedrag { get; set; }

        public int ToonFormulier { get; set; } = 0;

        public IEnumerable<ExterneInkoopViewModel> ViewModels { get; set; }

        public ExterneInkopenIndexViewModel()
        {
            Type = Type.Baat;
            Soort = Soort.ExterneInkoop;
        }

        public ExterneInkopenIndexViewModel(ExterneInkoop inkoop)
            : this()
        {
            Id = inkoop.Id;
            Beschrijving = inkoop.Beschrijving;
            Bedrag = inkoop.Bedrag;
        }
    }
}
