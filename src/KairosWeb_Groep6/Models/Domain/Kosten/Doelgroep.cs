namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class Doelgroep
    {
        #region Properties
        public DoelgroepSoort Soort { get; set; }

        // het minimum brutoloon dat hoort bij deze doelgroep, dit kan veranderen!
        public decimal MinBrutoloon { get; set; }

        // de doelgroepvermindering die hoort bij deze doelgroep, dit kan veranderen!
        public decimal StandaardDoelgroepVermindering { get; set; }
        #endregion

        #region Constructors
        public Doelgroep()
        {
            
        }

        public Doelgroep(DoelgroepSoort soort, decimal minBrutoloon, decimal standaardDoelgroepVermindering)
        {
            Soort = soort;
            MinBrutoloon = minBrutoloon;
            StandaardDoelgroepVermindering = standaardDoelgroepVermindering;
        }
        #endregion

        #region Controleermethoden
        private bool ControleerGegevensOntbreken(decimal brutoloon, decimal aantalUrenPerWeek, decimal aantalWerkuren, decimal patronaleBijdrage)
        {
            if (aantalWerkuren <= 0)
            {
                return false;
            }

            if (patronaleBijdrage <= 0)
            {
                return false;
            }

            if (brutoloon <= 0)
            {
                return false;
            }

            if (aantalUrenPerWeek <= 0)
            {
                return false;
            }

            if (MinBrutoloon <= 0)
            {
                return false;
            }

            if (StandaardDoelgroepVermindering <= 0)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Methods
        public decimal BerekenDoelgroepVermindering(decimal brutoloon, decimal aantalUrenPerWeek, 
            decimal aantalWerkuren, decimal patronaleBijdrage)
        {
            if (Soort == DoelgroepSoort.Andere)
            {// andere is steeds 0
                return 0;
            }

            if (ControleerGegevensOntbreken(brutoloon, aantalUrenPerWeek, aantalWerkuren, patronaleBijdrage))
            {
                // ALS bruto maandloon < minBrutoloon:
                // ((BEDRAG DOELGROEP / aantal uur voltijdse werkweek) *aantal uur dat medewerker werkt)/ 4
                decimal doelgroepvermindering = 0;

                if (brutoloon < MinBrutoloon)
                {
                    decimal verminderingPerUurWerkweek = StandaardDoelgroepVermindering / aantalWerkuren;
                    doelgroepvermindering = (verminderingPerUurWerkweek * aantalUrenPerWeek) / 4;
                }

                return doelgroepvermindering;
            }

            return 0;
        }

        public string GeefOmschrijving()
        {
            switch (Soort)
            {
                case DoelgroepSoort.LaaggeschooldTot25:
                    return "Wn's < 25 jaar laaggeschoold";
                case DoelgroepSoort.MiddengeschooldTot25:
                    return "Wn's < 25 jaar middengeschoold";
                case DoelgroepSoort.Tussen55En60:
                    return "Wn's ≥ 55 en < 60 jaar";
                case DoelgroepSoort.Vanaf60:
                    return "Wns ≥ 60 jaar";
                case DoelgroepSoort.Andere:
                    return "Andere";
                default:
                    return "";
            }
        }
        #endregion
    }
}
