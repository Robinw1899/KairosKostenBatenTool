using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Gebruiker
    {
        public int GebruikerId { get; set; }

        public string Naam { get; set; }

        public string Voornaam { get; set; }

        public string Emailadres { get; set; }

        public string Wachtwoord { get; set; }

        public bool IsAdmin { get; set; }

        public bool AlAangemeld { get; set; }

        public Organisatie Organisatie { get; set; }

        public ICollection<Analyse> Analyses { get; private set; }

        public Gebruiker()
        {
            
        }

        public Gebruiker(string naam, string voornaam, string emailadres, bool isAdmin)
        {
            Naam = naam;
            Voornaam = voornaam;
            Emailadres = emailadres;
            IsAdmin = isAdmin;
            AlAangemeld = false;
        }

        public Gebruiker(string naam, string voornaam, string emailadres, Organisatie organisatie)
            : this(naam, voornaam, emailadres, false)
        {
            Organisatie = organisatie;
            Analyses = new List<Analyse>();
        }
    }
}
