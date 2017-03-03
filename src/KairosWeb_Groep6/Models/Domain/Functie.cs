using System;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    /**
     * Dit komt overeen met kost 1.1 van de Excel.
     */
    public class Functie : KostOfBaat
    {
        public string Naam { get; set; }
        public double AantalUrenPerWeek { get; set; }
        public double BrutoMaandloonFulltime { get; set; }
        public double Ondersteuningspremie { get; set; }
        public Doelgroep? Doelgroep { get; set; }
        //props interface
        public ICollection<KolomWaarde> kolommen { get; set; }
        public ICollection<Rij> waarden { get; set; }
        public Type type { get; set; }

        public Functie()
        {
            kolommen = new List<KolomWaarde>();
            waarden = new List<Rij>();
        }

      
        public double berekenBrutoloonPerMaand()
        {
            throw new NotImplementedException();
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
