﻿using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class LoonkostExtensions
    {
        public static double GeefLoonkostVoorLoonkostVanId(this List<Loonkost> loonkosten, int id)
        {
            Loonkost loonkost = loonkosten.SingleOrDefault(l => l.Id == id);

            if (loonkost != null)
            {
                return loonkost.BerekenTotaleLoonkost();
            }

            return 0;
        }

        public static double GeefTotaalBrutoloonPerMaandAlleLoonkosten(this List<Loonkost> loonkosten)
        {
            double totaal = loonkosten.Sum(l => l.BerekenBrutoloonPerMaand());
            return totaal;
        }
    }
}
