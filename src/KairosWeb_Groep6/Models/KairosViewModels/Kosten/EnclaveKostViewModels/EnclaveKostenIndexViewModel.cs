using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.EnclaveKostViewModels
{
    public class EnclaveKostenIndexViewModel
    {
        #region Properties
        [HiddenInput]
        public int Id { get; set; }

        [HiddenInput]
        public Type Type { get; set; }

        [HiddenInput]
        public Soort Soort { get; set; }
        public string Beschrijving { get; set; }

        public double Bedrag { get; set; } // jaarbedrag van de huidige BegeleidingsKost

        public IEnumerable<EnclaveKostenViewModel> ViewModels { get; set; }
        #endregion

        #region Constructors
        public EnclaveKostenIndexViewModel()
        {

        }

        public EnclaveKostenIndexViewModel(EnclaveKost kost)
        {
            Type = kost.Type;
            Soort = kost.Soort;
            Beschrijving = kost.Beschrijving;
            Bedrag = kost.Bedrag;
        }
        #endregion
    }
}
