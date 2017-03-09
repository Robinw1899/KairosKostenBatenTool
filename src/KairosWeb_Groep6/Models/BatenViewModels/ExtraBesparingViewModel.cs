using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain.Baten;

namespace KairosWeb_Groep6.Models.BatenViewModels
{
    public class ExtraBesparingViewModel
    {
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; }
        public double Bedrag { get; set; }

        public ExtraBesparingViewModel()
        {
            
        }

        public ExtraBesparingViewModel(ExtraBesparing besparing)
        {
            Id = besparing.Id;
            Type = besparing.Type;
            Soort = besparing.Soort;
            Beschrijving = besparing.Beschrijving;
            Bedrag = besparing.Bedrag;
        }
    }
}
