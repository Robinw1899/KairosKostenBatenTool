using KairosWeb_Groep6.Models.Domain;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class ResultaatViewModel
    {
        #region Properties
        public int AnalyseId { get; set; }
        public IDictionary<Soort, decimal> Resultaten { get; set; }
        public decimal KostenTotaal { get; set; }
        public decimal BatenTotaal { get; set; }
        public decimal Totaal { get; set; }
        public bool AnalyseKlaar { get; set; }
        #endregion

        #region Constructors
        public ResultaatViewModel()
        {

        }
        #endregion
    }
}
