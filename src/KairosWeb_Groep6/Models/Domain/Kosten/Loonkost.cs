using KairosWeb_Groep6.Models.Domain.Extensions;

namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    public class Loonkost : KostOfBaat
    {
        #region Properties
       
        //Beschrijving = kolom "functie"

        public decimal AantalUrenPerWeek { get; set; }

        public override decimal Bedrag // = kolom "totale loonkost eerste jaar"
        {
            get { return 0; }
            set { } // setter wordt nooit gebruikt
        }

        public decimal BrutoMaandloonFulltime { get; set; }

        public decimal Ondersteuningspremie { get; set; }

        public int AantalMaandenIBO { get; set; }

        public decimal IBOPremie { get; set; }

        public DoelgroepSoort? Doelgroep { get; set; }

        #endregion

        #region Constructors
        public Loonkost()
        {
            Type = Type.Kost;
            Soort = Soort.Loonkost;
        }
        #endregion

        #region Controleermethoden
        private bool ControleerGegevensBrutoloonAanwezig(decimal aantalWerkuren, decimal patronaleBijdrage)
        {
            // als een gegeven niet aanwezig is, wordt een InvalidOperationException gegooid
            // controleer of de gegevens in Werkgever aanwezig zijn
            if (aantalWerkuren <= 0)
            {
                return false;
            }

            if (patronaleBijdrage <= 0)
            {
                return false;
            }

            if (BrutoMaandloonFulltime <= 0)
            {
                return false;
            }

            if (AantalUrenPerWeek <= 0)
            {
                return false;
            }

            return true;
        }

        private bool ControleerGegevensGemiddeldeVOPAanwezig()
        {
            if (Doelgroep == null)
            {
                return false;
            }

            if (Ondersteuningspremie < 0)
            {
                return false;
            }

            return true;
        }

        private bool ControleerAlleGegevensAanwezig(decimal aantalWerkuren, decimal patronaleBijdrage)
        {
            if (!ControleerGegevensBrutoloonAanwezig(aantalWerkuren, patronaleBijdrage))
            {
                return false;
            }
            if (!ControleerGegevensGemiddeldeVOPAanwezig())
            {
                return false;
            }
            if (AantalMaandenIBO <= 0)
            {
                return false;
            }

            if (IBOPremie <= 0)
            {
                return false;
            }

            return true;
        }
        #endregion

        #region Methods
        public decimal BerekenBrutoloonPerMaand(decimal aantalWerkuren, decimal patronaleBijdrage)
        {
            // ((bruto maandloon/aantal uur voltijdse werkweek) * aantal uur dat medewerker werkt) + 35% werkgeversbijdrage
            if (ControleerGegevensBrutoloonAanwezig(aantalWerkuren, patronaleBijdrage))
            {

                // bereken brutoloon per week van de werkgever
                decimal brutoloonPerWeekWerkgever = BrutoMaandloonFulltime / aantalWerkuren;
                // bereken brutoloon werknemer
                decimal brutoloonWerknemer = brutoloonPerWeekWerkgever * AantalUrenPerWeek;
                // tel patronale bijdrage erbij
                decimal procentPatronaleBijdrage = 1 + (patronaleBijdrage / 100);
                decimal brutoloon = brutoloonWerknemer * procentPatronaleBijdrage;
                
                return brutoloon;
            }

            return 0; // return 0 indien gegeven ontbreekt
        }

        public decimal BerekenGemiddeldeVOPPerMaand(decimal aantalWerkuren, decimal patronaleBijdrage)
        {
            //(bruto maandloon incl werkgeverslasten – maandelijkse doelgroepvermindering) * percentage VOP premie

            if (ControleerGegevensGemiddeldeVOPAanwezig())
            {
                decimal brutoloon = BerekenBrutoloonPerMaand(aantalWerkuren, patronaleBijdrage);
                decimal doelgroepvermindering = Doelgroep?.BerekenDoelgroepVermindering(BrutoMaandloonFulltime, AantalUrenPerWeek, aantalWerkuren, patronaleBijdrage) ?? 0;
                decimal gemiddeldeVOPPerMaand = (brutoloon - doelgroepvermindering) * (Ondersteuningspremie / 100);

                return gemiddeldeVOPPerMaand;
            }

            return 0; // return 0 indien gegeven ontbreekt
        }
        
        public decimal BerekenTotaleLoonkost(decimal aantalWerkuren, decimal patronaleBijdrage)
        {
            if (ControleerAlleGegevensAanwezig(aantalWerkuren, patronaleBijdrage))
            {
                // de rest wordt gecontroleerd in de andere methoden
                //(bruto loon per maand incl werkgeversbijdragen – gemiddelde VOP premie per maand – doelgroepvermindering per maand) 
                //* (13,92 – aantal maanden IBO) + totaalbedrag premie IBO

                decimal brutoloon = BerekenBrutoloonPerMaand(aantalWerkuren, patronaleBijdrage);
                decimal gemVOP = BerekenGemiddeldeVOPPerMaand(aantalWerkuren, patronaleBijdrage);
                decimal doelgroepvermindering =
                    Doelgroep?.BerekenDoelgroepVermindering(BrutoMaandloonFulltime, AantalUrenPerWeek, aantalWerkuren, patronaleBijdrage) ?? 0;
                // linkerdeel van de berekening (voor de * )
                decimal linkerfactor = brutoloon - gemVOP - doelgroepvermindering;

                // rechterdeel van de berekening (na de *)
                decimal rechterfactor = 13.92M - AantalMaandenIBO;

                decimal loonkost = (linkerfactor * rechterfactor) + IBOPremie;

                return loonkost;
            }

            return 0; // return 0 indien gegeven ontbreekt
        }
        #endregion
    }
}
