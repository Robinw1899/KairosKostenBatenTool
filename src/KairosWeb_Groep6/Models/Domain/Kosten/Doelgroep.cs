namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class Doelgroep
    {
        #region Properties
        public int DoelgroepId { get; set; }

        public string Omschrijving { get; set; }

        // het minimum brutoloon dat hoort bij deze doelgroep
        public decimal MinBrutoloon { get; set; }

        // de doelgroepvermindering die hoort bij deze doelgroep
        public decimal StandaardDoelgroepVermindering { get; set; }

        // deze boolean duidt aan of de doelgroep verwijderd is door de admin
        // op de plaatsen waar deze gebruikt wordt, blijft alles normaal
        // in een nieuwe analyse staat deze niet meer
        public bool Verwijderd { get; set; }
        #endregion

        #region Constructors
        public Doelgroep()
        {
            
        }

        public Doelgroep(string omschrijving, decimal minBrutoloon, decimal standaardDoelgroepVermindering)
        {
            Omschrijving = omschrijving;
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
            //if (Omschrijving.Equals("Andere"))
            //{
            //    return 0;
            //}

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

        public override string ToString()
        {
            return Omschrijving ?? "";
        }
        #endregion
    }
}
