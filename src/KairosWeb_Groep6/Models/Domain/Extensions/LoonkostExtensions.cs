using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class LoonkostExtensions
    {
        public static decimal GeefTotaalBrutolonenPerJaarAlleLoonkosten(List<Loonkost> loonkosten,
            decimal aantalWerkuren, decimal patronaleBijdrage)
        {
            decimal totaal = loonkosten.Sum(l => l.BerekenBrutoloonPerMaand(aantalWerkuren, patronaleBijdrage));
            return totaal * 12; // * 12 omdat totaal de som is per maand
        }

        public static decimal GeefTotaalAlleLoonkosten(List<Loonkost> loonkosten,
            decimal aantalWerkuren, decimal patronaleBijdrage)
        {
            decimal totaal = loonkosten.Sum(l => l.BerekenTotaleLoonkost(aantalWerkuren, patronaleBijdrage));
            return totaal;
        }
    }
}
