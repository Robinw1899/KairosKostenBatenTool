using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;


namespace KairosWeb_Groep6.Models.BatenViewModels
{
    public class ExterneInkoopViewModel
    {
        public int Id { get; set; }
        public Type Type { get; set; }
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
