﻿using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class LoonkostExtensions
    {
        public static double GeefTotaalBrutolonenPerJaarAlleLoonkosten(this List<Loonkost> loonkosten, 
            int aantalWerkuren, int patronaleBijdrage)
        {
            double totaal = loonkosten.Sum(l => l.BerekenBrutoloonPerMaand(aantalWerkuren, patronaleBijdrage));
            return totaal * 12; // * 12 omdat totaal de som is per maand
        }
    }
}
