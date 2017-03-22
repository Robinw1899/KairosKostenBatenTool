using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.ProjectModel;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.GereedschapsKostenViewModels
{
    public class GereedschapsKostenViewModel
    {
        [Required]
        [HiddenInput]
        public int Id { get; set; }
        [Required]
        [HiddenInput]
        public Domain.Type Type { get; set; }
        [Required]
        [HiddenInput]
        public Soort Soort { get; set; }
        [Required]
        public string Beschrijving { get; set; }
        [Required(ErrorMessage = "Gelieve een bedrag op te geven.")]
        [Range(0, double.MaxValue, ErrorMessage = "Gelieve enkel een positief getal in te geven voor het bedrag")]
        public double Bedrag { get; set; }
        public IEnumerable<GereedschapsKostenViewModel> ViewModels { get; internal set; }

        public GereedschapsKostenViewModel()
        {
            
        }
        public GereedschapsKostenViewModel(GereedschapsKost kost)
        {
            Id = kost.Id;
            Type = kost.Type;
            Soort = kost.Soort;
            Beschrijving = kost.Beschrijving;
            Bedrag = kost.Bedrag;
            
        }
    }
}
