using System;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Departement
    {
        #region Properties

        public int DepartementId { get; set; }
        public string Naam { get; set; } = "";

        public Werkgever Werkgever { get; set; }
        #endregion

        #region Constructors

        public Departement()
        {
            
        }

        public Departement(string naam)
        {
            Naam = naam;
        }
        #endregion

        #region Methods
        public bool Contains(string zoekterm)
        {
            if (Naam.IndexOf(zoekterm, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }

            return false;
        }
        #endregion
    }
}
