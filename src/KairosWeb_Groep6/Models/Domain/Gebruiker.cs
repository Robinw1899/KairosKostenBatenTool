namespace KairosWeb_Groep6.Models.Domain
{
    public class Gebruiker
    {
        public string Naam { get; set; }

        public string Voornaam { get; set; }

        public string Emailadres { get; set; }

        public bool IsAdmin { get; set; }

        public Gebruiker(string naam, string voornaam, string emailadres, bool isAdmin)
        {
            Naam = naam;
            Voornaam = voornaam;
            Emailadres = emailadres;
            IsAdmin = isAdmin;
        }
    }
}
