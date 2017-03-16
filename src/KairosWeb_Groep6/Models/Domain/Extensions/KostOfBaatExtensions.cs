using System.Collections.Generic;
using System.Linq;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class KostOfBaatExtensions
    {
        public static KostOfBaat GetBy(List<KostOfBaat> lijst, int id)
        {
            return lijst
                .SingleOrDefault(e => e.Id == id);
        }

        public static double GeefTotaal<T>(List<T> lijst) where T : KostOfBaat
        {
            return lijst
                .Sum(v => v.Bedrag);
        }
    }
}
