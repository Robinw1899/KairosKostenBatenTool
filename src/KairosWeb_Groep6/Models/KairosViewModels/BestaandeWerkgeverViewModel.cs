using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class BestaandeWerkgeverViewModel
    {
        #region Properties
        public IEnumerable<WerkgeverViewModel> Werkgevers { get; set; }

        public bool FirstLoad { get; set; }

        public int Index { get; set; }
        public int Aantal { get; set; }
        public bool ShowVolgende { get; set; }
        public bool ShowVorige { get; set; }
        #endregion

        public BestaandeWerkgeverViewModel()
        {
            Werkgevers = new List<WerkgeverViewModel>();
            FirstLoad = true;
        }
    }
}
