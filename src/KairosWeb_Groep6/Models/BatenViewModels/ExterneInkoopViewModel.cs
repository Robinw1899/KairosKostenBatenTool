using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Microsoft.AspNetCore.Mvc;

namespace KairosWeb_Groep6.Models.BatenViewModels
{
    public class ExterneInkoopViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        [HiddenInput]
        public Type Type { get; set; }
        [HiddenInput]
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; }
        public double Bedrag { get; set; } // = kolom "jaarbedrag"

        public ExterneInkoopViewModel()
        {
            
        }

        public ExterneInkoopViewModel(ExterneInkoop inkoop)
        {
            Id = inkoop.Id;
            Type = inkoop.Type;
            Soort = inkoop.Soort;
            Beschrijving = inkoop.Beschrijving;
            Bedrag = inkoop.Bedrag;
        }
    }
}
