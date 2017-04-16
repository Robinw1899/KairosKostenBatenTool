using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Baten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public class MedewerkerNiveauBaatExtensions
    {
        public static decimal GeefTotaal(List<MedewerkerNiveauBaat> baten,
            decimal aantalWerkuren, decimal patronaleBijdrage)
        {
            decimal totaal = baten.Sum(l => l.BerekenTotaleLoonkostPerJaar(aantalWerkuren, patronaleBijdrage));
            return totaal;
        }
    }
}
