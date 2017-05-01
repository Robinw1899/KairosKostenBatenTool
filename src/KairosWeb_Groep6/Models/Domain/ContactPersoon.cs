namespace KairosWeb_Groep6.Models.Domain
{
    public class ContactPersoon
    {

        #region Properties
        public int ContactPersoonId { get; set; }

        public string Naam { get; set; }

        public string Voornaam { get; set; }

        public string Emailadres { get; set; }

        #endregion

        #region Constructors

        public ContactPersoon()
        {
            
        }
        public ContactPersoon(string voornaam, string naam, string email)
        {
            Voornaam = voornaam;
            Naam = naam;
            Emailadres = email;
        }
        #endregion
    }
}
