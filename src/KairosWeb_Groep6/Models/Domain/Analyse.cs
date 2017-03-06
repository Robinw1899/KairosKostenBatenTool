    using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Analyse
    {
        public Werkgever Werkgever { get; set; }
        public IDictionary<Soort, List<KostOfBaat>> KostenEnBaten { get; }

        #region Kosten
        public List<KostOfBaat> Loonkosten => KostenEnBaten[Soort.Loonkost];

        public List<KostOfBaat> EnclaveKosten => KostenEnBaten[Soort.EnclaveKost];

        public List<KostOfBaat> VoorbereidingsKosten => KostenEnBaten[Soort.VoorbereidingsKost];

        public List<KostOfBaat> InfrastructuurKosten => KostenEnBaten[Soort.InfrastructuurKost];

        public List<KostOfBaat> GereedschapsKosten => KostenEnBaten[Soort.GereedschapsKost];

        public List<KostOfBaat> OpleidingsKosten => KostenEnBaten[Soort.OpleidingsKost];

        public List<KostOfBaat> BegeleidingsKosten => KostenEnBaten[Soort.BegeleidingsKost];
        #endregion

        #region Baten
        public List<KostOfBaat> MedewerkersZelfdeNiveauBaat => KostenEnBaten[Soort.MedewerkersZelfdeNiveau];

        public List<KostOfBaat> MedewerkersHogerNiveauBaat => KostenEnBaten[Soort.MedewerkersHogerNiveau];

        public List<KostOfBaat> UitzendKrachtBesparingen => KostenEnBaten[Soort.UitzendkrachtBesparing];

        public List<KostOfBaat> ExtraOmzet => KostenEnBaten[Soort.ExtraOmzet];

        public List<KostOfBaat> ExtraProductiviteit => KostenEnBaten[Soort.ExtraProductiviteit];

        public List<KostOfBaat> OverurenBesparing => KostenEnBaten[Soort.OverurenBesparing];

        public List<KostOfBaat> ExterneInkopen => KostenEnBaten[Soort.ExterneInkoop];

        public List<KostOfBaat> Subsidies => KostenEnBaten[Soort.Subsidie];

        public List<KostOfBaat> ExtraBesparingen => KostenEnBaten[Soort.ExtraBesparing];
        #endregion

        #region Constructors
        public Analyse()
        {
            KostenEnBaten = new Dictionary<Soort, List<KostOfBaat>>();
        }
        #endregion

        #region Methods

        public void VoegKostOfBaatToe(Soort soort, KostOfBaat kostOfBaat)
        {
            List<KostOfBaat> lijst = KostenEnBaten[soort];
            lijst.Add(kostOfBaat);
        }
        public void VerwijderKostOfBaatToe(Soort soort, KostOfBaat kostOfBaat)
        {
            List<KostOfBaat> lijst = KostenEnBaten[soort];
            lijst.Remove(kostOfBaat);
        }
        #endregion
    }
}
