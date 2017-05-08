using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class BestaandeWerkgeverViewModel
    {
        #region Properties
        public IEnumerable<WerkgeverViewModel> Werkgevers { get; set; }

        public bool FirstLoad { get; set; }   
        
        public bool ToonMeer { get; set; }
        #endregion

        public BestaandeWerkgeverViewModel()
        {
            Werkgevers = new List<WerkgeverViewModel>();
            FirstLoad = true;
            ToonMeer = false;
        }
    }
}
