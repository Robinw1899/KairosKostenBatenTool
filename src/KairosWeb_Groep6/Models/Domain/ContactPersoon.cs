using System.ComponentModel.DataAnnotations.Schema;

namespace KairosWeb_Groep6.Models.Domain
{
    public class ContactPersoon : Persoon
    {

        #region Properties
        public bool IsHoofdContactPersoon { get; set; }
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
