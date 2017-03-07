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
        public int Id { get; set; }
        public string Beschrijving { get; set; } // = kolom "functie"

        public double AantalUrenPerWeek { get; set; }

        public double Bedrag // = kolom "totale loonkost eerste jaar"
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

        public Type Type { get; set; }

        public Soort Soort { get; set; }

        
        #endregion

        #region Constructors
        public Loonkost()
        {
            Type = Type.Kost;
            Soort = Soort.Loonkost;
        }
        #endregion

        #region Controleermethoden
        private void ControleerGegevensBrutoloonAanwezig()
        {
            // als een gegeven niet aanwezig is, wordt een InvalidOperationException gegooid
            // controleer of de gegevens in Werkgever aanwezig zijn
            if (Werkgever.AantalWerkuren == 0)
            {
                throw new InvalidOperationException("Gelieve het aantal werkuren bij de werkgever in te vullen.");
            }

            if (Werkgever.PatronaleBijdrage <= 0)
            {
                throw new InvalidOperationException("Gelieve de patronale bijdrage bij de werkgever in te vullen.");
            }

            if (BrutoMaandloonFulltime <= 0)
            {
                throw new InvalidOperationException("Gelieve een waarde groter dan 0 voor het bruto maandloon van een fulltime werknemer in te vullen.");
            }

            if (AantalUrenPerWeek <= 0)
            {
                throw new InvalidOperationException("Gelieve een waarde groter dan 0 voor het aantal uren per week in te vullen.");
            }            
        }

        private void ControleerGegevensGemiddeldeVOPAanwezig()
        {
            if (Doelgroep == null)
            {
                throw new InvalidOperationException($"Gelieve de doelgroep in te vullen bij de loonkost met functie {Beschrijving}");
            }
        }

        private void ControleerGegevensIBOAanwezig()
        {
            if (AantalMaandenIBO <= 0)
            {
                throw new InvalidOperationException("Gelieve het aantal maanden IBO in te vullen.");
            }

            if (IBOPremie <= 0)
            {
                throw new InvalidOperationException("Gelieve de totale productiviteitspremie IBO in te vullen.");
            }
        }
        #endregion

        #region Methods
        public double BerekenBrutoloonPerMaand()
        {
            // ((bruto maandloon/aantal uur voltijdse werkweek) * aantal uur dat medewerker werkt) + 35% werkgeversbijdrage
            ControleerGegevensBrutoloonAanwezig();

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
            ControleerGegevensGemiddeldeVOPAanwezig();
            //(bruto maandloon incl werkgeverslasten – maandelijkse doelgroepvermindering) * percentage VOP premie
            double gemiddeldeVOPPerMaand = 0;

            if (Ondersteuningspremie > 0)
            {
                double brutoloon = BerekenBrutoloonPerMaand();
                double doelgroepvermindering = Doelgroep?.BerekenDoelgroepVermindering(BrutoMaandloonFulltime, AantalUrenPerWeek) ?? 0;
                gemiddeldeVOPPerMaand = (brutoloon - doelgroepvermindering) * Ondersteuningspremie;
            }

            return gemiddeldeVOPPerMaand;
        }
        
        public double BerekenTotaleLoonkost()
        {
            ControleerGegevensIBOAanwezig(); // de rest wordt gecontroleerd in de andere methoden
            //(bruto loon per maand incl werkgeversbijdragen – gemiddelde VOP premie per maand – doelgroepvermindering per maand) 
            //* (13,92 – aantal maanden IBO) + totaalbedrag premie IBO
            double loonkost = 0;

            double brutoloon = BerekenBrutoloonPerMaand();
            double gemVOP = BerekenGemiddeldeVOPPerMaand();
            double doelgroepvermindering = Doelgroep?.BerekenDoelgroepVermindering(BrutoMaandloonFulltime, AantalUrenPerWeek) ?? 0;
            // linkerdeel van de berekening (voor de * )
            double linkerfactor = brutoloon - gemVOP - doelgroepvermindering;

            // rechterdeel van de berekening (na de *)
            double rechterfactor = 13.92 - AantalMaandenIBO;

            loonkost = (linkerfactor * rechterfactor) + IBOPremie;

            return loonkost;
        }
        #endregion
    }
}
