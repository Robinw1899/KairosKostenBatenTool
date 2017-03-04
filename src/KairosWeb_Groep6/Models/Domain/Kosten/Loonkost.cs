namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    /**
     * Dit komt overeen met kost 1.1 van de Excel.
     */
    public class Loonkost : KostOfBaat
    {
        #region Properties
        public int Id { get; set; }
        public string Functie { get; set; }

        public double AantalUrenPerWeek { get; set; }

        public double BrutoMaandloonFulltime { get; set; }

        public double Ondersteuningspremie { get; set; }

        public int AantalMaandenIBO { get; set; }

        public double IBOPremie { get; set; }

        public Doelgroep? Doelgroep { get; set; }

        public Type Type { get; set; }

        public Soort Soort { get; set; }
        #endregion

        #region Constructors
        public Loonkost()
        {
            Type = Type.KOST;
            Soort = Soort.Loonkost;
        }
        #endregion

        #region Methods
        public double BerekenBrutoloonPerMaand()
        {
            // ((bruto maandloon/aantal uur voltijdse werkweek) * aantal uur dat medewerker werkt) + 35% werkgeversbijdrage

            // bereken brutoloon per week van de werkgever
            double brutoloonPerWeekWerkgever = BrutoMaandloonFulltime / Werkgever.AantalWerkuren;
            // bereken brutoloon werknemer
            double brutoloonWerknemer = brutoloonPerWeekWerkgever * AantalUrenPerWeek;
            // tel patronale bijdrage erbij
            double brutoloon = brutoloonWerknemer * (1 + Werkgever.PatronaleBijdrage);

            return brutoloon;
        }
        public double BerekenGemiddeldeVOPPerMaand()
        {
            //(bruto maandloon incl werkgeverslasten – maandelijkse doelgroepvermindering) * percentage VOP premie
            double gemiddeldeVOPPerMaand = 0;

            double brutoloon = BerekenBrutoloonPerMaand();
            double doelgroepvermindering = Doelgroep?.BerekenDoelgroepVermindering(brutoloon, AantalUrenPerWeek) ?? 0;
            gemiddeldeVOPPerMaand = (brutoloon - doelgroepvermindering) * Ondersteuningspremie;

            return gemiddeldeVOPPerMaand;
        }
        
        public double BerekenTotaleLoonkost()
        {
            //(bruto loon per maand incl werkgeversbijdragen – gemiddelde VOP premie per maand – doelgroepvermindering per maand) 
            //* (13,92 – aantal maanden IBO) + totaalbedrag premie IBO
            double loonkost = 0;

            double brutoloon = BerekenBrutoloonPerMaand();
            double gemVOP = BerekenGemiddeldeVOPPerMaand();
            double doelgroepvermindering = Doelgroep?.BerekenDoelgroepVermindering(brutoloon, AantalUrenPerWeek) ?? 0;
            // linkerdeel van de berekening (voor de * )
            double linkerfactor = brutoloon - gemVOP - doelgroepvermindering;

            // rechterdeel van de berekening (na de *)
            double rechterfactor = (13.92 - AantalMaandenIBO) + IBOPremie;

            loonkost = linkerfactor * rechterfactor;

            return loonkost;
        }
        #endregion
    }
}
