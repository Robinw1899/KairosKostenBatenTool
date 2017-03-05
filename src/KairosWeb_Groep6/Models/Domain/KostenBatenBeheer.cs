using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class KostenBatenBeheer
    {
        private IDictionary<Soort, List<KostOfBaat>> KostenEnBaten { get; }

        public List<KostOfBaat> Loonkosten => KostenEnBaten[Soort.Loonkost];

        public List<KostOfBaat> EnclaveKosten => KostenEnBaten[Soort.EnclaveKost];

        public List<KostOfBaat> VoorbereidingsKosten => KostenEnBaten[Soort.VoorbereidingsKost];

        public List<KostOfBaat> InfrastructuurKosten => KostenEnBaten[Soort.InfrastructuurKost];

        public List<KostOfBaat> GereedschapsKosten => KostenEnBaten[Soort.GereedschapsKost];

        public List<KostOfBaat> OpleidingsKosten => KostenEnBaten[Soort.OpleidingsKost];

        public List<KostOfBaat> BegeleidingsKosten => KostenEnBaten[Soort.BegeleidingsKost];

        public KostenBatenBeheer()
        {
            KostenEnBaten = new Dictionary<Soort, List<KostOfBaat>>();
        }
    }
}
