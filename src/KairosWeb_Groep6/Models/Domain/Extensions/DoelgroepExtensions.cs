using System;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain.Extensions
{
    public static class DoelgroepExtensions
    {
        public static decimal BerekenDoelgroepVermindering(this Doelgroep doelgroep, decimal brutoloon,
                                                        decimal aantalUrenPerWeek, decimal aantalWerkuren, decimal patronaleBijdrage)
        {
            ControleerGegevensOntbreken(brutoloon, aantalUrenPerWeek, aantalWerkuren, patronaleBijdrage);
            decimal minBrutoloon = GetMinBrutoLoon(doelgroep);

            // ALS bruto maandloon < minBrutoloon:
            // ((BEDRAG DOELGROEP / aantal uur voltijdse werkweek) *aantal uur dat medewerker werkt)/ 4
            decimal doelgroepvermindering = 0;

            if (brutoloon < minBrutoloon)
            {
                decimal verminderingPerUurWerkweek = (decimal) doelgroep / aantalWerkuren;
                doelgroepvermindering = (verminderingPerUurWerkweek * aantalUrenPerWeek) / 4;
            }

            return doelgroepvermindering;
        }

        private static void ControleerGegevensOntbreken(decimal brutoloon, decimal aantalUrenPerWeek, decimal aantalWerkuren, decimal patronaleBijdrage)
        {
            if (aantalWerkuren <= 0)
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

        public static decimal GetMinBrutoLoon(Doelgroep doelgroep)
        {
            decimal minBrutoloon = 0;

            switch (doelgroep)
            {
                case Doelgroep.LaaggeschooldTot25:
                case Doelgroep.MiddengeschooldTot25:
                    minBrutoloon = 2500;
                    break;
                case Doelgroep.Tussen55En60:
                case Doelgroep.Vanaf60:
                    minBrutoloon = 4466.66M;
                    break;
            }

            return minBrutoloon;
        }

        public static string GeefOmschrijving(Doelgroep? doelgroep)
        {
            switch (doelgroep)
            {
                case Doelgroep.LaaggeschooldTot25:
                    return "Wn's < 25 jaar laaggeschoold";
                case Doelgroep.MiddengeschooldTot25:
                    return "Wn's < 25 jaar middengeschoold";
                case Doelgroep.Tussen55En60:
                    return "Wn's ≥ 55 en < 60 jaar";
                case Doelgroep.Vanaf60:
                    return "Wns ≥ 60 jaar";
                case Doelgroep.Andere:
                    return "Andere";
                default:
                    return "";
            }
        }
    }
}
