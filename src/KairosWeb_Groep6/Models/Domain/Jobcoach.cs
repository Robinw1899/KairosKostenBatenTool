using System;
using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Jobcoach : Gebruiker
    {
        public int JobcoachId { get; set; }

        public Organisatie Organisatie { get; set; }

        public ICollection<Analyse> Analyses { get; private set; }

        public Jobcoach(string naam, string voornaam, string emailadres, Organisatie organisatie)
            : base(naam, voornaam, emailadres, false)
        {
            Organisatie = organisatie;
            Analyses = new List<Analyse>();
        }
    }
}
