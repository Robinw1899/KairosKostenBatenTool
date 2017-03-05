using System;

namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public static class DoelgroepExtensions
    {
        public static double BerekenDoelgroepVermindering(this Doelgroep doelgroep, double brutoloon,
                                                        double aantalUrenPerWeek)
        {
            double minBrutoloon = GetMinBrutoLoon(doelgroep);

            // ALS bruto maandloon < minBrutoloon:
            // ((BEDRAG DOELGROEP / aantal uur voltijdse werkweek) *aantal uur dat medewerker werkt)/ 4
            double doelgroepvermindering = 0;

            if (brutoloon < minBrutoloon)
            {
                double verminderingPerUurWerkweek = (double) doelgroep / Werkgever.AantalWerkuren;
                doelgroepvermindering = (verminderingPerUurWerkweek * aantalUrenPerWeek) / 4;
            }

            return doelgroepvermindering;
        }

        private static double GetMinBrutoLoon(Doelgroep doelgroep)
        {
            double minBrutoloon = 0;

            switch (doelgroep)
            {
                case Doelgroep.LaaggeschooldTot25:
                case Doelgroep.MiddengeschooldTot25:
                    minBrutoloon = 2500;
                    break;
                case Doelgroep.Tussen55En60:
                case Doelgroep.Vanaf60:
                    minBrutoloon = 4466.66;
                    break;
            }

            return minBrutoloon;
        }
    }

    public enum Doelgroep
    {
        LaaggeschooldTot25 = 1550,
        MiddengeschooldTot25 = 1000,
        Tussen55En60 = 1150,
        Vanaf60 = 1500,
        Andere = 0
    }
}