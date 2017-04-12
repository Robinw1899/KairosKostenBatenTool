using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class BestaandeWerkgeverViewModel
    {
        #region Properties
        public IEnumerable<WerkgeverViewModel> Werkgevers { get; set; }
        #endregion
    }
}
