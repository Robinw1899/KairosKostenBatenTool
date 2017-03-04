using System;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    /**
     * Dit komt overeen met kost 1.1 van de Excel.
     */
    public class Functie
    {
        public string Naam { get; set; }

        public double AantalUrenPerWeek { get; set; }

        public double BrutoMaandloonFulltime { get; set; }

        public double Ondersteuningspremie { get; set; }

        public int AantalMaandenIBO { get; set; }

        public double IBOPremie { get; set; }

        public Doelgroep? Doelgroep { get; set; }

        public Type Type { get; set; }

        public Functie()
        {
            Type = Type.KOST;
        }

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
        public double berekenGemiddeldeVOPPerMaand()
        {
            throw new NotImplementedException();
        }
        public double berekenDoelgroepVermindering()
        {
            throw new NotImplementedException();
        }
        public double berekenTotaleProductiviteitsPremieIBO()
        {
            throw new NotImplementedException();
        }
        public double berekenTotaleLoonkost(String naam)
        {
            throw new NotImplementedException();
        }
        public double berekenTotaleLoonkostAlleFuncties()
        {
            throw new NotImplementedException();
        }
        public double berekenTotalBrutoloonPerJaarAlleFuncties()
        {
            throw new NotImplementedException();
        }

        //methoden interface KostOfBaat
        
        public double getBedrag(int rijNr)
        {
            throw new NotImplementedException();
        }

        public double berekenTotaal()
        {
            throw new NotImplementedException();
        }
    }
}
