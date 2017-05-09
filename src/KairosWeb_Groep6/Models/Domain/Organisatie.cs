using System;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Organisatie
    {
        #region Properties
        public int OrganisatieId { get; set; }

        public string Naam { get; set; }

        public string Straat { get; set; }

        public int Nummer { get; set; }

        public string Bus { get; set; } = "";

        public int Postcode { get; set; }

        public string Gemeente { get; set; }
        #endregion

        #region Constructors
        public Organisatie()
        {
            
        }

        public Organisatie(string naam, string straat, int nummer, string bus, int postcode, string gemeente)
        {
            Naam = naam;
            Straat = straat;
            Nummer = nummer;
            Bus = bus;
            Postcode = postcode;
            Gemeente = gemeente;
        }
        #endregion

        #region Methods
        public bool Equals(Organisatie other)
        {
            if (!string.Equals(Naam, other.Naam, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (other.Straat != null && !string.Equals(Straat, other.Straat, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (Nummer != other.Nummer)
            {
                return false;
            }

            if (other.Bus != null && !string.Equals(Bus, other.Bus, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (Postcode != other.Postcode)
            {
                return false;
            }

            if (!string.Equals(Gemeente, other.Gemeente, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
        #endregion
    }
}
