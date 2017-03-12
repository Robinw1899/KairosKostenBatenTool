using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public string Beschrijving { get; set; }
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
