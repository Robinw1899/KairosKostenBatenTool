using KairosWeb_Groep6.Models.Domain;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class ResultaatViewModel
    {
        #region Properties
        public IDictionary<Soort, double> Resultaten { get; set; }
        public double KostenTotaal { get; set; }
        public double BatenTotaal { get; set; }
        public double Totaal { get; set; }
        #endregion

        #region Constructors
        public ResultaatViewModel()
        {

        }

        public ResultaatViewModel(IDictionary<Soort, double> resultaten)
        {
            Resultaten = resultaten;

        }
        #endregion
    }
}
