using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.OpleidingsKosten
{
    public class OpleidingsKostIndexViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public Domain.Type Type { get; set; }
        [Required]
        public Soort Soort { get; set; }
        [Required(ErrorMessage = "Gelieve een beschrijving op te geven")]
        public string Beschrijving { get; set; }
        [Required(ErrorMessage = "Gelieve een bedrag op te geven")]
        [Range(0, Double.MaxValue, ErrorMessage = "Gelieve enkel een positief getal op te geven voor het bedrag")]
        public double Bedrag { get; set; }

        public IEnumerable<OpleidingsKostViewModel> ViewModels { get; internal set; }

        public OpleidingsKostIndexViewModel()
        {

        }
        public OpleidingsKostIndexViewModel(OpleidingsKost kost)
        {
            Id = kost.Id;
            Type = kost.Type;
            Soort = kost.Soort;
            Beschrijving = kost.Beschrijving;
            Bedrag = kost.Bedrag;
        }
    }
}
