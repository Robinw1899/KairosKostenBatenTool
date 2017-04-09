using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Baten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public class MedewerkerNiveauBaatExtensions
    {
        public static double GeefTotaal(List<MedewerkerNiveauBaat> baten,
            double aantalWerkuren, double patronaleBijdrage)
        {
            double totaal = baten.Sum(l => l.BerekenTotaleLoonkostPerJaar(aantalWerkuren, patronaleBijdrage));
            return totaal;
        }
    }
}
