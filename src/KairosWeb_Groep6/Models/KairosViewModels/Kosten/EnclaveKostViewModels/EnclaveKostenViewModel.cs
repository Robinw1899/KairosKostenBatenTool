using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.EnclaveKostViewModels
{
    public class EnclaveKostenViewModel
    {
        #region Properties
        public int Id { get; set; }

        public Type Type { get; set; }

        public Soort Soort { get; set; }
        public string Beschrijving { get; set; }

        public double Bedrag { get; set; } // jaarbedrag van de huidige BegeleidingsKost
        #endregion

        #region Constructors
        public EnclaveKostenViewModel()
        {

        }
        public EnclaveKostenViewModel(EnclaveKost kost)
        {
            Id = kost.Id;
            Type = kost.Type;
            Soort = kost.Soort;
            Bedrag = kost.Bedrag;
            Beschrijving = kost.Beschrijving;
        }
        #endregion
    }
}
