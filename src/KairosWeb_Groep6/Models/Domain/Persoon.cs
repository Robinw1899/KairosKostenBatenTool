namespace KairosWeb_Groep6.Models.Domain
{
    public class Persoon
    {
        #region Properties
        public int PersoonId { get; set; }

        public string Naam { get; set; }

        public string Voornaam { get; set; }

        public string Emailadres { get; set; }
        #endregion

        #region Constructors

        public Persoon()
        {
            
        }
        public Persoon(string voornaam, string naam, string email)
        {
            Voornaam = voornaam;
            Naam = naam;
            Emailadres = email;
        }
        #endregion
    }
}
