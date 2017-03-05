using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class KostOfBaatExtensions
    {
        public static KostOfBaat GetBy(this List<KostOfBaat> lijst, int id)
        {
            return lijst
                .SingleOrDefault(e => e.Id == id);
        }

        public static double GeefTotaal(this List<KostOfBaat> lijst)
        {
            return lijst
                .Sum(v => v.Bedrag);
        }
    }
}
