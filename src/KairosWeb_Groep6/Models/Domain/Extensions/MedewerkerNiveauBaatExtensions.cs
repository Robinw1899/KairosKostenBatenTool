using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Baten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class MedewerkerNiveauBaatExtensions
    {
        public static MedewerkerNiveauBaat GetBy(this List<MedewerkerNiveauBaat> lijst, int id)
        {
            return lijst
                .SingleOrDefault(e => e.Id == id);
        }

        public static double GeefTotaal(this List<MedewerkerNiveauBaat> lijst)
        {
            return lijst
                .Sum(v => v.Bedrag);
        }
    }
}
