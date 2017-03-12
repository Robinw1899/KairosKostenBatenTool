using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.BatenViewModels
{
    public class OverurenBesparingViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        [HiddenInput]
        public Type Type { get; set; }
        [HiddenInput]
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; } // wordt niet gebruikt
        public double Bedrag { get; set; }

        public OverurenBesparingViewModel()
        {
            
        }

        public OverurenBesparingViewModel(OverurenBesparing besparing)
        {
            Id = besparing.Id;
            Type = besparing.Type;
            Soort = besparing.Soort;
            Beschrijving = besparing.Beschrijving;
            Bedrag = besparing.Bedrag;
        }
    }
}
