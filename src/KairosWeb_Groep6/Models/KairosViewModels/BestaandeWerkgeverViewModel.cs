using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class BestaandeWerkgeverViewModel
    {
        #region Properties
        public IEnumerable<WerkgeverViewModel> Werkgevers { get; set; }

        public bool FirstLoad { get; set; }

        public int BeginIndex { get; set; }

        public int EindIndex { get; set; }
        public int Totaal { get; set; }
        public bool ShowVolgende { get; set; }
        public bool ShowVorige { get; set; }

        public string zoekstring { get; set; }
        #endregion

        public BestaandeWerkgeverViewModel()
        {
            Werkgevers = new List<WerkgeverViewModel>();
            FirstLoad = true;
        }
    }
}
