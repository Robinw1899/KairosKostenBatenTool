using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain
{
    public class KostenBatenBeheer
    {
        private IDictionary<Soort, List<KostOfBaat>> KostenEnBaten { get; }

        public List<Loonkost> Loonkosten
        {
            get
            {
                List<KostOfBaat> loonkosten = KostenEnBaten[Soort.Loonkost];

                return loonkosten
                    .Select(k => (Loonkost)k)
                    .ToList();
            }
        }

        public List<EnclaveKost> EnclaveKosten
        {
            get
            {
                List<KostOfBaat> enclavekosten = KostenEnBaten[Soort.EnclaveKost];

                return enclavekosten
                    .Select(k => (EnclaveKost)k)
                    .ToList();
            }
        }

        public List<VoorbereidingsKost> VoorbereidingsKosten
        {
            get
            {
                List<KostOfBaat> voorbereindskosten = KostenEnBaten[Soort.VoorbereidingsKost];

                return voorbereindskosten
                    .Select(k => (VoorbereidingsKost)k)
                    .ToList();
            }
        }

        //private IList<KostOfBaat> KostenEnBaten { get; set; }

        public KostenBatenBeheer()
        {
            KostenEnBaten = new Dictionary<Soort, List<KostOfBaat>>();
        }
    }
}
