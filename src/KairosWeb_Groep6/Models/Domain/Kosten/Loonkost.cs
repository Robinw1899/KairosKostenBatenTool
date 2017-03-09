using System;
using KairosWeb_Groep6.Models.Domain.Extensions;

namespace KairosWeb_Groep6.Models.Domain.Kosten
{
    /**
     * Dit komt overeen met kost 1.1 van de Excel.
     */
    public class Loonkost : KostOfBaat
    {
        #region Properties
       
        //Beschrijving = kolom "functie"

        public double AantalUrenPerWeek { get; set; }

        public override double Bedrag // = kolom "totale loonkost eerste jaar"
        {
            get
            {
                return BerekenTotaleLoonkost();
            }
            set { } // setter wordt nooit gebruikt
        }

        public double BrutoMaandloonFulltime { get; set; }

        public double Ondersteuningspremie { get; set; }

        public int AantalMaandenIBO { get; set; }

        public double IBOPremie { get; set; }

        public Doelgroep? Doelgroep { get; set; }

        #endregion

        #region Constructors
        public Loonkost()
        {
            Type = Type.Kost;
            Soort = Soort.Loonkost;
        }
        #endregion

        #region Controleermethoden
        private bool ControleerGegevensBrutoloonAanwezig()
        {
            // als een gegeven niet aanwezig is, wordt een InvalidOperationException gegooid
            // controleer of de gegevens in Werkgever aanwezig zijn
            if (Werkgever.AantalWerkuren == 0)
            {
                return false;
            }

            if (Werkgever.PatronaleBijdrage <= 0)
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

        private bool ControleerAlleGegevensAanwezig()
        {
            if (!ControleerGegevensBrutoloonAanwezig())
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
        public double BerekenBrutoloonPerMaand()
        {
            // ((bruto maandloon/aantal uur voltijdse werkweek) * aantal uur dat medewerker werkt) + 35% werkgeversbijdrage
            if (ControleerGegevensBrutoloonAanwezig())
            {

                // bereken brutoloon per week van de werkgever
                double brutoloonPerWeekWerkgever = BrutoMaandloonFulltime / Werkgever.AantalWerkuren;
                // bereken brutoloon werknemer
                double brutoloonWerknemer = brutoloonPerWeekWerkgever * AantalUrenPerWeek;
                // tel patronale bijdrage erbij
                double brutoloon = brutoloonWerknemer * (1 + Werkgever.PatronaleBijdrage);

                return brutoloon;
            }

            return 0; // return 0 indien gegeven ontbreekt
        }

        public double BerekenGemiddeldeVOPPerMaand()
        {
            //(bruto maandloon incl werkgeverslasten – maandelijkse doelgroepvermindering) * percentage VOP premie

            if (ControleerGegevensGemiddeldeVOPAanwezig())
            {
                double brutoloon = BerekenBrutoloonPerMaand();
                double doelgroepvermindering = Doelgroep?.BerekenDoelgroepVermindering(BrutoMaandloonFulltime, AantalUrenPerWeek) ?? 0;
                double gemiddeldeVOPPerMaand = (brutoloon - doelgroepvermindering) * Ondersteuningspremie;

                return gemiddeldeVOPPerMaand;
            }

            return 0; // return 0 indien gegeven ontbreekt
        }
        
        public double BerekenTotaleLoonkost()
        {
            if (ControleerAlleGegevensAanwezig())
            {
                // de rest wordt gecontroleerd in de andere methoden
                //(bruto loon per maand incl werkgeversbijdragen – gemiddelde VOP premie per maand – doelgroepvermindering per maand) 
                //* (13,92 – aantal maanden IBO) + totaalbedrag premie IBO

                double brutoloon = BerekenBrutoloonPerMaand();
                double gemVOP = BerekenGemiddeldeVOPPerMaand();
                double doelgroepvermindering =
                    Doelgroep?.BerekenDoelgroepVermindering(BrutoMaandloonFulltime, AantalUrenPerWeek) ?? 0;
                // linkerdeel van de berekening (voor de * )
                double linkerfactor = brutoloon - gemVOP - doelgroepvermindering;

                // rechterdeel van de berekening (na de *)
                double rechterfactor = 13.92 - AantalMaandenIBO;

                double loonkost = (linkerfactor * rechterfactor) + IBOPremie;

                return loonkost;
            }

            return 0; // return 0 indien gegeven ontbreekt
        }
        #endregion
    }
}
