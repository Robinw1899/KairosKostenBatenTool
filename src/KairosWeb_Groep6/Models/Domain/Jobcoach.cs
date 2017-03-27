using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Jobcoach
    {
        public int JobcoachId { get; set; }

        public string Naam { get; set; }

        public string Voornaam { get; set; }

        public string Emailadres { get; set; }

        public string Wachtwoord { get; set; }

        public bool AlAangemeld { get; set; }

        public Organisatie Organisatie { get; set; }

        public ICollection<Analyse> Analyses { get; set; } = new List<Analyse>();

        public Jobcoach()
        {
            
        }

        public Jobcoach(string naam, string voornaam, string emailadres)
        {
            Naam = naam;
            Voornaam = voornaam;
            Emailadres = emailadres;
            AlAangemeld = false;
        }

        public Jobcoach(string naam, string voornaam, string emailadres, Organisatie organisatie)
            : this(naam, voornaam, emailadres)
        {
            Organisatie = organisatie;
            Analyses = new List<Analyse>();
        }
    }
}
