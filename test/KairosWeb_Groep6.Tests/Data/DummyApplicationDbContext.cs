using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Microsoft.DotNet.ProjectModel.FileSystemGlobbing.Internal.PathSegments;

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

        public List<ExtraKost> ExtraKosten { get; set; }

        public List<MedewerkerNiveauBaat> MedewerkerNiveauBaten { get; set; }

        public List<Subsidie> Subsidies { get; set; }

        public DummyApplicationDbContext()
        {
            MaakOrganisaties();
            MaakGebruikers();
            MaakLoonkosten();
            MaakExtraKosten();
            MaakMedewerkerNiveauBaten();
            MaakSubsidies();
        }

        private void MaakOrganisaties()
        {
            HoGent = new Organisatie("HoGent", "Arbeidstraat", 10, 9300, "Aalst");
            Colruyt = new Organisatie("Colruyt", "Weggevoerdenstraat", 55, 9404, "Ninove");
        }

        private void MaakGebruikers()
        {
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

        private void MaakLoonkosten()
        {
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

        private void MaakExtraKosten()
        {
            ExtraKosten = new List<ExtraKost>();
            ExtraKosten.Add(new ExtraKost { Id = 1, Bedrag = 150, Beschrijving = "Stagekosten" });
            ExtraKosten.Add(new ExtraKost { Id = 2, Bedrag = 1000, Beschrijving = "Uitrusting" });
            ExtraKosten.Add(new ExtraKost { Id = 3, Bedrag = 400, Beschrijving = "Boeken en ander studiemateriaal" });
        }

        private void MaakMedewerkerNiveauBaten()
        {
            MedewerkerNiveauBaten = new List<MedewerkerNiveauBaat>();
            MedewerkerNiveauBaten.Add(new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau) { Id = 1, Uren = 35, BrutoMaandloonFulltime = 2300});
            MedewerkerNiveauBaten.Add(new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau) { Id = 2, Uren = 30, BrutoMaandloonFulltime = 2000});
            MedewerkerNiveauBaten.Add(new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau) { Id = 3, Uren = 37, BrutoMaandloonFulltime = 3250});
        }

        private void MaakSubsidies()
        {
            Subsidies = new List<Subsidie>();
            Subsidies.Add(new Subsidie { Id = 1, Bedrag = 200 });
            Subsidies.Add(new Subsidie { Id = 2, Bedrag = 1500 });
        }
    }
}
