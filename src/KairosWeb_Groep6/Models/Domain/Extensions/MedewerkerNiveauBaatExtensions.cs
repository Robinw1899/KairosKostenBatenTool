using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Baten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public class MedewerkerNiveauBaatExtensions
    {
        public static double GeefTotaalBrutolonenPerJaarAlleLoonkosten(List<MedewerkerNiveauBaat> baten,
            int aantalWerkuren, double patronaleBijdrage)
        {
            double totaal = baten.Sum(l => l.BerekenTotaleLoonkostPerJaar(aantalWerkuren, patronaleBijdrage));
            return totaal;
        }
    }
}
