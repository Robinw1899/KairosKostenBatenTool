using System;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Jobcoach
    {
        public int JobcoachId { get; set; }
        public string Naam { get; set; }

        public string Voornaam { get; set; }

        public string Emailadres { get; set; }

        public Organisatie Organisatie { get; set; }

        public ICollection<Analyse> analyses { get; private set; }

        public Jobcoach(string naam, string voornaam, string emailadres, Organisatie organisatie)
        {
            Naam = naam;
            Voornaam = voornaam;
            Emailadres = emailadres;
            Organisatie = organisatie;
            analyses = new List<Analyse>();
        }
    }
}
