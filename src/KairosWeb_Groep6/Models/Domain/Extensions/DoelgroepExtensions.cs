using System;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class DoelgroepExtensions
    {
        public static double BerekenDoelgroepVermindering(this Doelgroep doelgroep, double brutoloon,
                                                        double aantalUrenPerWeek, int aantalWerkuren, double patronaleBijdrage)
        {
            ControleerGegevensOntbreken(brutoloon, aantalUrenPerWeek, aantalWerkuren, patronaleBijdrage);
            double minBrutoloon = GetMinBrutoLoon(doelgroep);

            // ALS bruto maandloon < minBrutoloon:
            // ((BEDRAG DOELGROEP / aantal uur voltijdse werkweek) *aantal uur dat medewerker werkt)/ 4
            double doelgroepvermindering = 0;

            if (brutoloon < minBrutoloon)
            {
                double verminderingPerUurWerkweek = (double)doelgroep / aantalWerkuren;
                doelgroepvermindering = (verminderingPerUurWerkweek * aantalUrenPerWeek) / 4;
            }

            return doelgroepvermindering;
        }

        private static void ControleerGegevensOntbreken(double brutoloon, double aantalUrenPerWeek, int aantalWerkuren, double patronaleBijdrage)
        {
            if (aantalWerkuren == 0)
            {
                throw new InvalidOperationException("Gelieve het aantal werkuren per week in te vullen bij de werkgever.");
            }

            if (patronaleBijdrage <= 0)
            {
                throw new InvalidOperationException("Gelieve de patronale bijdrage in te vullen bij de werkgever.");
            }

            if (brutoloon <= 0)
            {
                throw new InvalidOperationException("Gelieve het brutoloon in te vullen bij de functie.");
            }

            if (aantalUrenPerWeek <= 0)
            {
                throw new InvalidOperationException("Gelieve het aantal uren per week in te vullen bij de functie.");
            }
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
}
