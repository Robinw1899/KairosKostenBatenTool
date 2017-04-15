using System.Collections.Generic;
using System;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Werkgever
    {
        #region Properties
       
        public int WerkgeverId { get; set; }
     
        public string Naam { get; set; } = "";

        public string Straat { get; set; } = "";

        public int? Nummer { get; set; }

        public string Bus { get; set; } = "";

        public int Postcode { get; set; }

        public string Gemeente { get; set; } = "";

        public decimal AantalWerkuren { get; set; }

        public decimal PatronaleBijdrage { get; set; } = 35M;
      
        public List<ContactPersoon> ContactPersonen { get; set; } = new List<ContactPersoon>();
             
        public List<Departement> Departementen { get; set; }
        #endregion

        #region Constructors
        public Werkgever()

        {
            
        }

        public Werkgever(string naam, string straat, int nummer, string bus, int postcode, string gemeente, int aantalWerkuren)
            : this(naam, straat, nummer, bus, postcode, gemeente, aantalWerkuren, 35)
        {
            
        }

        public Werkgever(string naam, string straat, int nummer, string bus, int postcode, string gemeente, int aantalWerkuren, double patronaleBijdrage)
        {
            Naam = naam;
            Straat = straat;
            Nummer = nummer;
            Bus = bus;
            Postcode = postcode;
            Gemeente = gemeente;
            AantalWerkuren = aantalWerkuren;
            PatronaleBijdrage = patronaleBijdrage;          
        }
        #endregion

        #region Methods

        public bool Contains(string zoekterm)
        {
            if (Naam.IndexOf(zoekterm, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            if (Gemeente.IndexOf(zoekterm, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            return false;
        }

        protected bool Equals(Werkgever other)
        {
            // gegenereerde code
            if (other == null)
            {
                return false;
            }

            return string.Equals(Naam, other.Naam) 
                && string.Equals(Straat, other.Straat) 
                && Nummer == other.Nummer 
                && string.Equals(Bus, other.Bus) 
                && Postcode == other.Postcode 
                && string.Equals(Gemeente, other.Gemeente) 
                && AantalWerkuren.Equals(other.AantalWerkuren);
        }

        public override int GetHashCode()
        {
            // gegenereerde code
            unchecked
            {
                var hashCode = (Naam != null ? Naam.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Straat != null ? Straat.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Nummer.GetHashCode();
                hashCode = (hashCode * 397) ^ (Bus != null ? Bus.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ Postcode;
                hashCode = (hashCode * 397) ^ (Gemeente != null ? Gemeente.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ AantalWerkuren.GetHashCode();
                return hashCode;
            }
        }

        #endregion
    }
}
