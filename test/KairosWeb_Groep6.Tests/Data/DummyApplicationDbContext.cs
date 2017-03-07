using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;

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

        public Loonkost Poetsvrouw { get; set; }

        public Loonkost Secretaresse { get; set; }

        public Loonkost Postbode { get; set; }

        public List<Loonkost> Loonkosten { get; set; }

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

            Poetsvrouw = new Loonkost
            {
                Id = 1,
                BrutoMaandloonFulltime = 1800,
                AantalUrenPerWeek = 37,
                Doelgroep = Doelgroep.LaaggeschooldTot25,
                Ondersteuningspremie = 0.20D,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0D
            };

            Secretaresse = new Loonkost
            {
                Id = 2,
                BrutoMaandloonFulltime = 2200,
                AantalUrenPerWeek = 23,
                Doelgroep = Doelgroep.MiddengeschooldTot25,
                Ondersteuningspremie = 0.20D,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0D
            };

            Postbode = new Loonkost
            {
                Id = 3,
                BrutoMaandloonFulltime = 1900,
                AantalUrenPerWeek = 35,
                Doelgroep = Doelgroep.Tussen55En60,
                Ondersteuningspremie = 0.20D,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0D
            };

            Loonkosten = new List<Loonkost>
            {
                Poetsvrouw,
                Secretaresse,
                Postbode
            };
        }
    }
}
