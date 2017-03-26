using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;
using System.Linq;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class AnalyseViewModel
    {
        #region Properties
        public double KostenTotaal { get; set; }

        public double BatenTotaal { get; set; }

        public double NettoResultaat { get; set; }

        public KostenIndexViewModel KostenInfo { get; set; }

        public BatenIndexViewModel BatenInfo { get; set; }
        #endregion

        #region Constructor
        public AnalyseViewModel()
        {

        }

        public AnalyseViewModel(Analyse analyse)
        {
            KostenInfo = new KostenIndexViewModel(analyse);
            BatenInfo = new BatenIndexViewModel(analyse);

            KostenTotaal =  analyse.GeefTotalenKosten()
                                     .Sum(x => x.Value);

            BatenTotaal = analyse.GeefTotalenBaten()
                                    .Sum(x => x.Value);

            NettoResultaat = BatenTotaal - KostenTotaal;
        }
        #endregion
    }
}