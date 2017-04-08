﻿using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Tests.Data
{
    public class DummyApplicationDbContext
    {
        public IEnumerable<Jobcoach> Gebruikers { get; set; }

        public Jobcoach Dimmy { get; set; }

        public Jobcoach Thomas { get; set; }

        public Jobcoach Robin { get; set; }

        public Organisatie HoGent { get; set; }

        public Organisatie Colruyt { get; set; }

        public Loonkost Poetsvrouw { get; set; }

        public Loonkost Secretaresse { get; set; }

        public Loonkost Postbode { get; set; }

        public List<Loonkost> Loonkosten { get; set; }

        public List<ExtraKost> ExtraKosten { get; set; }

        public List<BegeleidingsKost> BegeleidingsKosten { get; set; }

        public List<MedewerkerNiveauBaat> MedewerkerNiveauBaten { get; set; }

        public List<UitzendKrachtBesparing> UitzendKrachtBesparingen { get; set; }

        public List<Subsidie> Subsidies { get; set; }

        public DummyApplicationDbContext()
        {
            MaakOrganisaties();
            MaakGebruikers();
            MaakLoonkosten();
            MaakExtraKosten();
            MaakMedewerkerNiveauBaten();
            MaakSubsidies();
            MaakUitzendKrachtBesparingen();
            MaakBegeleidingsKosten();
        }

        private void MaakOrganisaties()
        {
            HoGent = new Organisatie("HoGent", "Arbeidstraat", 10, "", 9300, "Aalst");
            Colruyt = new Organisatie("Colruyt", "Weggevoerdenstraat", 55, "", 9404, "Ninove");
        }

        private void MaakGebruikers()
        {
            Thomas = new Jobcoach("Aelbrecht", "Thomas", "thomas.aelbrecht@gmail.com", HoGent) { PersoonId = 1 };
            Robin = new Jobcoach("Coppens", "Robin", "robbin.coppens@gmail.com", HoGent) { PersoonId = 2 };
            Dimmy = new Jobcoach("Maenhout", "Dimmy", "dimmy.maenhout@test.be", Colruyt) { PersoonId = 3 };

            Gebruikers = new List<Jobcoach>
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
            ExtraKosten = new List<ExtraKost>
            {
                new ExtraKost {Id = 1, Bedrag = 150, Beschrijving = "Stagekosten"},
                new ExtraKost {Id = 2, Bedrag = 1000, Beschrijving = "Uitrusting"},
                new ExtraKost {Id = 3, Bedrag = 400, Beschrijving = "Boeken en ander studiemateriaal"}
            };
        }

        private void MaakMedewerkerNiveauBaten()
        {
            MedewerkerNiveauBaten = new List<MedewerkerNiveauBaat>
            {
                new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
                {
                    Id = 1,
                    Uren = 35,
                    BrutoMaandloonFulltime = 2300
                },
                new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
                {
                    Id = 2,
                    Uren = 30,
                    BrutoMaandloonFulltime = 2000
                },
                new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
                {
                    Id = 3,
                    Uren = 37,
                    BrutoMaandloonFulltime = 3250
                },
                new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
                {
                    Id = 4,
                    Uren = 23,
                    Bedrag = 2500
                },
                new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
                {
                    Id = 5,
                    Uren = 37,
                    Bedrag = 3500
                },
                new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
                {
                    Id = 6,
                    Uren = 28,
                    Bedrag = 2750
                }
            };
        }

        private void MaakSubsidies()
        {
            Subsidies = new List<Subsidie>
            {
                new Subsidie {Id = 1, Bedrag = 200},
                new Subsidie {Id = 2, Bedrag = 1500},
                new Subsidie {Id = 3, Bedrag = 3500},
                new Subsidie {Id = 4, Bedrag = 15000},
                new Subsidie {Id = 5, Bedrag = 6750}
            };
        }

        public void MaakUitzendKrachtBesparingen()
        {
            UitzendKrachtBesparingen = new List<UitzendKrachtBesparing>
            {
                new UitzendKrachtBesparing
                {
                    Id = 1,
                    Beschrijving = "Tuinier",
                    Bedrag = 2500
                },
                new UitzendKrachtBesparing
                {
                    Id = 2,
                    Beschrijving = "Klusjesman",
                    Bedrag = 3500
                },
                new UitzendKrachtBesparing
                {
                    Id = 3,
                    Beschrijving = "WC-madam",
                    Bedrag = 2750
                },
                new UitzendKrachtBesparing
                {
                    Id = 4,
                    Beschrijving = "Boekhouder",
                    Bedrag = 5400
                },
                new UitzendKrachtBesparing
                {
                    Id = 5,
                    Beschrijving = "Sorteerder",
                    Bedrag = 3420
                }
            };
        }

        public void MaakBegeleidingsKosten()
        {
            BegeleidingsKosten = new List<BegeleidingsKost>
            {
                new BegeleidingsKost
                {
                    Id = 1,
                    Uren = 37,
                    BrutoMaandloonBegeleider = 3400
                },
                new BegeleidingsKost
                {
                    Id = 2,
                    Uren = 25,
                    BrutoMaandloonBegeleider = 2500
                },
                new BegeleidingsKost
                {
                    Id = 3,
                    Uren = 30,
                    BrutoMaandloonBegeleider = 2870
                }
            };
        }
    }
}
