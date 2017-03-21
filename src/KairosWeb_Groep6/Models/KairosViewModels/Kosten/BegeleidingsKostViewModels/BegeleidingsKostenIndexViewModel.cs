using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten.BegeleidingsKostViewModels
{
    public class BegeleidingsKostenIndexViewModel
    {
        #region Properties
        public int Id { get; set; }

        public Type Type { get; set; }

        public Soort Soort { get; set; }

        public double Uren { get; set; }

        public double BrutoMaandloonBegeleider { get; set; }

        public double Bedrag { get; set; } // jaarbedrag van de huidige BegeleidingsKost

        public IEnumerable<BegeleidingsKostViewModel> ViewModels { get; set; }
        #endregion
    }
}
