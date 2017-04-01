using KairosWeb_Groep6.Models.Domain;

namespace KairosWeb_Groep6.Models.KairosViewModels.Baten
{
    public class BatenIndexViewModel
    {
        #region Properties
        public bool MedewerkersZelfdeNiveauBaatIngevuld { get; set; }

        public bool MedewerkersHogerNiveauBaatIngevuld { get; set; }

        public bool UitzendKrachtBesparingenIngevuld { get; set; }

        public bool ExtraOmzetIngevuld { get; set; }

        public bool ExtraProductiviteitIngevuld { get; set; }

        public bool OverurenBesparingIngevuld { get; set; }

        public bool ExterneInkopenIngevuld { get; set; }

        public bool SubsidieIngevuld { get; set; }

        public bool ExtraBesparingenIngevuld { get; set; }

        public int AantalIngevuld { get; set; }
        #endregion

        #region Constructors
        public BatenIndexViewModel()
        {

        }

        public BatenIndexViewModel(Analyse analyse)
        {
            if (analyse.MedewerkersZelfdeNiveauBaat.Count != 0)
            {
                MedewerkersZelfdeNiveauBaatIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.MedewerkersHogerNiveauBaat.Count != 0)
            {
                MedewerkersHogerNiveauBaatIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.UitzendKrachtBesparingen.Count != 0)
            {
                UitzendKrachtBesparingenIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.ExtraOmzet != null && analyse.ExtraOmzet.Bedrag > 0)
            {
                ExtraOmzetIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.ExtraProductiviteit != null && analyse.ExtraProductiviteit.Bedrag > 0)
            {
                ExtraProductiviteitIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.OverurenBesparing != null && analyse.OverurenBesparing.Bedrag > 0)
            {
                OverurenBesparingIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.ExterneInkopen.Count != 0)
            {
                ExterneInkopenIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.Subsidie != null && analyse.Subsidie.Bedrag > 0)
            {
                SubsidieIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.ExtraBesparingen.Count != 0)
            {
                ExtraBesparingenIngevuld = true;
                AantalIngevuld += 1;
            }
        }
        #endregion
    }
}
