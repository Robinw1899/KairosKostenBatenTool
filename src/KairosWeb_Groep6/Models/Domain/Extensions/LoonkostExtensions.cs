using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class LoonkostExtensions
    {
        public static double GeefTotaalBrutolonenPerJaarAlleLoonkosten(this List<Loonkost> loonkosten)
        {
            double totaal = loonkosten.Sum(l => l.BerekenBrutoloonPerMaand());
            return totaal * 12; // * 12 omdat totaal de som is per maand
        }
    }
}
