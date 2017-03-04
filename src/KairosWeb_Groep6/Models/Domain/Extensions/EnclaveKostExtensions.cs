using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class EnclaveKostExtensions
    {
        public static EnclaveKost GetBy(this List<EnclaveKost> enclaveKosten, int id)
        {
            return enclaveKosten
                .SingleOrDefault(e => e.Id == id);
        }

        public static double GeefJaarbedragVanId(this List<EnclaveKost> enclaveKosten, int id)
        {
            return enclaveKosten
                .SingleOrDefault(e => e.Id == id)
                .Jaarbedrag;
        }

        public static double GeefTotaal(this List<EnclaveKost> enclaveKosten)
        {
            return enclaveKosten.Sum(e => e.Jaarbedrag);
        }
    }
}
