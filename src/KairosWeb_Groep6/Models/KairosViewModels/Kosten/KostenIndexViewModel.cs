using KairosWeb_Groep6.Models.Domain;

namespace KairosWeb_Groep6.Models.KairosViewModels.Kosten
{
    public class KostenIndexViewModel
    {
        #region Properties
        public bool LoonkostenIngevuld { get; set; } = false;

        public bool EnclaveKostenIngevuld { get; set; } = false;

        public bool VoorbereidingsKostenIngevuld { get; set; } = false;

        public bool InfrastructuurKostenIngevuld { get; set; } = false;

        public bool GereedschapsKostenIngevuld { get; set; } = false;

        public bool OpleidingsKostenIngevuld { get; set; } = false;

        public bool BegeleidingsKostenIngevuld { get; set; } = false;

        public bool ExtraKostenIngevuld { get; set; } = false;

        public int AantalIngevuld { get; set; } = 0;

        public int AantalKosten { get; set; } = 8;
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
                AantalIngevuld += 1;
            }

            if (analyse.EnclaveKosten.Count != 0)
            {
                EnclaveKostenIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.VoorbereidingsKosten.Count != 0)
            {
                VoorbereidingsKostenIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.InfrastructuurKosten.Count != 0)
            {
                InfrastructuurKostenIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.GereedschapsKosten.Count != 0)
            {
                GereedschapsKostenIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.OpleidingsKosten.Count != 0)
            {
                OpleidingsKostenIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.BegeleidingsKosten.Count != 0)
            {
                BegeleidingsKostenIngevuld = true;
                AantalIngevuld += 1;
            }

            if (analyse.ExtraKosten.Count != 0)
            {
                ExtraKostenIngevuld = true;
                AantalIngevuld += 1;
            }
        }
        #endregion
    }
}
