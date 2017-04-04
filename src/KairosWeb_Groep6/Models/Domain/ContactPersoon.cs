using System.ComponentModel.DataAnnotations.Schema;

namespace KairosWeb_Groep6.Models.Domain
{
    public class ContactPersoon : Persoon
    {
        #region Properties            
        public Werkgever Werkgever { get; set; } // nodig voor one-to-one-relatie
        #endregion

        #region Constructors

        public ContactPersoon()
        {
            
        }
        public ContactPersoon(string voornaam, string naam, string email)
            : base(voornaam, naam, email)
        {

        }
        #endregion
    }
}
