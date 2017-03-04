using System;
using System.Collections.Generic;
using System.Linq;

namespace KairosWeb_Groep6.Models.Domain
{
    public class KostenBatenBeheer
    {
        private IList<KostOfBaat> KostenEnBaten { get; set; }

        private IList<Functie> Functies { get; }
        
        public KostenBatenBeheer()
        {
            Functies = new List<Functie>();
            throw new NotImplementedException();
        }

        public double GeefLoonkostVoorFunctieMetNaam(string naam)
        {
            Functie functie = Functies.SingleOrDefault(f => f.Naam.Equals(naam));
            
            if (functie != null)
            {
                return functie.BerekenTotaleLoonkost();
            }

            return 0;
        }

        public double GeefTotaleLoonkostAlleFunties()
        {
            double somAlleLoonkosten = Functies.Sum(f => f.BerekenTotaleLoonkost());
            return somAlleLoonkosten;
        }

        public void VoegFunctieToe(Functie functie)
        {
            Functies.Add(functie);
        }

        public void VerwijderFunctie(Functie functie)
        {
            Functies.Remove(functie);
        }
    }
}
