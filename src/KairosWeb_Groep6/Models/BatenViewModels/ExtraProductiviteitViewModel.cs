using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;


namespace KairosWeb_Groep6.Models.BatenViewModels
{
    public class ExtraProductiviteitViewModel : IEnumerable
    {
        public int Id { get; set; }
        public Type Type { get; set; }
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; } // wordt niet gebruikt
        public double Bedrag { get; set; }

        public ExtraProductiviteitViewModel()
        {

        }

        public ExtraProductiviteitViewModel(ExtraProductiviteit productiviteit)
        {
            Id = productiviteit.Id;
            Type = productiviteit.Type;
            Soort = productiviteit.Soort;
            Beschrijving = productiviteit.Beschrijving;
            Bedrag = productiviteit.Bedrag;
        }

        public IEnumerator GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}
