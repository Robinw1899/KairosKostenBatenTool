using KairosWeb_Groep6.Models.Domain;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class ResultaatViewModel
    {
        #region Properties
        public IDictionary<Soort, double> Resultaten { get; set; }
        #endregion

        #region Constructors
        public ResultaatViewModel()
        {

        }
        #endregion
    }
}
