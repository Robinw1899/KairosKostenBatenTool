using KairosWeb_Groep6.Models.Domain;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten
{
    public class KostenIndexViewModel
    {
        #region Properties
        public bool LoonkostenIngevuld { get; set; }

        public bool EnclaveKostenIngevuld { get; set; }

        public bool VoorbereidingsKostenIngevuld { get; set; }

        public bool PersoneelsKostenIngevuld { get; set; }

        public bool GereedschapsKostenIngevuld { get; set; }

        public bool OpleidingsKostenIngevuld { get; set; }

        public bool BegeleidingsKostenIngevuld { get; set; }

        public bool ExtraKostenIngevuld { get; set; }
        #endregion

        #region Constructor
        public KostenIndexViewModel()
        {

        }

        public KostenIndexViewModel(Analyse analyse)
        {
            if (analyse.Loonkosten.Count != 0)
            {
                LoonkostenIngevuld = true;
            }

            if (analyse.EnclaveKosten.Count != 0)
            {
                EnclaveKostenIngevuld = true;
            }

            if (analyse.VoorbereidingsKosten.Count != 0)
            {
                VoorbereidingsKostenIngevuld = true;
            }

            if (analyse.PersoneelsKosten.Count != 0)
            {
                PersoneelsKostenIngevuld = true;
            }

            if (analyse.GereedschapsKosten.Count != 0)
            {
                GereedschapsKostenIngevuld = true;
            }

            if (analyse.OpleidingsKosten.Count != 0)
            {
                OpleidingsKostenIngevuld = true;
            }

            if (analyse.BegeleidingsKosten.Count != 0)
            {
                BegeleidingsKostenIngevuld = true;
            }

            if (analyse.ExtraKosten.Count != 0)
            {
                ExtraKostenIngevuld = true;
            }
        }
        #endregion
    }
}
