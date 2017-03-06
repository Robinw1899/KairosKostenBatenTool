using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;

namespace KairosWeb_Groep6.Tests.Data
{
    public class DummyApplicationDbContext
    {
        public IEnumerable<Gebruiker> Gebruikers { get; set; }

        public Gebruiker Dimmy { get; set; }

        public Gebruiker Thomas { get; set; }

        public Gebruiker Robin { get; set; }

        public Organisatie HoGent { get; set; }

        public Organisatie Colruyt { get; set; }

        public DummyApplicationDbContext()
        {
            HoGent = new Organisatie("HoGent", "Arbeidstraat", 10, 9300, "Aalst");
            Colruyt = new Organisatie("Colruyt", "Weggevoerdenstraat", 55, 9404, "Ninove");

            Thomas = new Gebruiker("Aelbrecht", "Thomas", "thomas.aelbrecht@gmail.com", HoGent) { GebruikerId = 1 };
            Robin = new Gebruiker("Coppens", "Robin", "robbin.coppens@gmail.com", HoGent) { GebruikerId = 2 };
            Dimmy = new Gebruiker("Maenhout", "Dimmy", "dimmy.maenhout@test.be", Colruyt) { GebruikerId = 3 };

            Gebruikers = new List<Gebruiker>
            {
                Thomas,
                Robin,
                Dimmy
            };
        }
    }
}
