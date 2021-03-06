﻿using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Tests.Data
{
    public class DummyApplicationDbContext
    {
        #region Jobcoaches - Organisatie
        public IEnumerable<Jobcoach> Gebruikers { get; set; }

        public Jobcoach Dimmy { get; set; }

        public Jobcoach Thomas { get; set; }

        public Jobcoach Robin { get; set; }

        public Organisatie HoGent { get; set; }

        public Organisatie Colruyt { get; set; }
        #endregion

        #region Kosten

        public Doelgroep Laaggeschoold { get; set; } = new Doelgroep("Wn's < 25 jaar laaggeschoold", 2500M, 1550M);

        public Loonkost Poetsvrouw { get; set; }

        public Loonkost Secretaresse { get; set; }

        public Loonkost Postbode { get; set; }

        public List<Loonkost> Loonkosten { get; set; }

        public List<ExtraKost> ExtraKosten { get; set; }

        public List<BegeleidingsKost> BegeleidingsKosten { get; set; }

        public List<OpleidingsKost> OpleidingsKosten { get; set; }

        public List<PersoneelsKost> PersoneelsKosten { get; set; }

        public List<GereedschapsKost> GereedschapsKosten { get; set; }

        public List<VoorbereidingsKost> VoorbereidingsKosten { get; set; }

        public List<EnclaveKost> EnclaveKosten { get; set; }
        #endregion

        #region Baten
        public List<MedewerkerNiveauBaat> MedewerkerNiveauBaten { get; set; }

        public List<UitzendKrachtBesparing> UitzendKrachtBesparingen { get; set; }

        public List<ExterneInkoop> ExterneInkopen { get; set; }

        public Subsidie Subsidie { get; set; }

        public LogistiekeBesparing LogistiekeBesparing { get; set; }

        public ExtraOmzet ExtraOmzet { get; set; }

        public ExtraProductiviteit ExtraProductiviteit { get; set; }

        public OverurenBesparing OverurenBesparing { get; set; }

        public List<ExtraBesparing> ExtraBesparingen { get; set; }
        #endregion

        #region Departement
        public Departement Aldi { get; set; }
        #endregion

        #region Constructor
        public DummyApplicationDbContext()
        {
            /* ANDERE */
            Aldi = new Departement("Verkoop")
            {
                Werkgever = new Werkgever("ALDI", "Arbeidstraat", 14, "", 9300, "Aalst", 37),
                ContactPersoon = new ContactPersoon("Thomas", "Aelbrecht", "thomas@test.com")
            };

            MaakOrganisaties();
            MaakJobcoaches();

            /* KOSTEN */
            MaakGereedschapsKosten();
            MaakInfrastructuurKosten();
            MaakVoorbereidingsKosten();
            MaakOpleidingsKosten();
            MaakEnclaveKosten();
            MaakLoonkosten();
            MaakExtraKosten();
            MaakBegeleidingsKosten();

            /* BATEN */
            MaakMedewerkerNiveauBaten();
            MaakSubsidie();
            MaakUitzendKrachtBesparingen();
            MaakLogistiekeBesparing();
            MaakExterneInkopen();
            MaakExtraBesparingen();

            ExtraOmzet = new ExtraOmzet
            {
                Besparing = 5,
                JaarbedragOmzetverlies = 12000
            };

            ExtraProductiviteit = new ExtraProductiviteit
            {
                Bedrag = 6470
            };

            OverurenBesparing = new OverurenBesparing
            {
                Bedrag = 34570
            };
        }
        #endregion

        #region Methoden Jobcoach - Organisatie
        private void MaakOrganisaties()
        {
            HoGent = new Organisatie("HoGent", "Arbeidstraat", 10, "", 9300, "Aalst");
            Colruyt = new Organisatie("Colruyt", "Weggevoerdenstraat", 55, "", 9404, "Ninove");
        }

        private void MaakJobcoaches()
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
        #endregion

        #region Methoden kosten
        private void MaakLoonkosten()
        {
            string laaggeschoold = "Wn's < 25 jaar laaggeschoold";
            string middengeschoold = "Wn's < 25 jaar middengeschoold";
            string tussen55En60 = "Wn's ≥ 55 en < 60 jaar";

            Poetsvrouw = new Loonkost
            {
                Id = 1,
                BrutoMaandloonFulltime = 1800,
                AantalUrenPerWeek = 37,
                Doelgroep = new Doelgroep(laaggeschoold, 2500M, 1550M),
                Ondersteuningspremie = 20,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0M
            };

            Secretaresse = new Loonkost
            {
                Id = 2,
                BrutoMaandloonFulltime = 2200,
                AantalUrenPerWeek = 23,
                Doelgroep = new Doelgroep(middengeschoold, 2500M, 1000M),
                Ondersteuningspremie = 20,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0M
            };

            Postbode = new Loonkost
            {
                Id = 3,
                BrutoMaandloonFulltime = 1900,
                AantalUrenPerWeek = 35,
                Doelgroep = new Doelgroep(tussen55En60, 2500M, 1150M),
                Ondersteuningspremie = 20,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0M
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

        private void MaakBegeleidingsKosten()
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

        private void MaakOpleidingsKosten()
        {
            OpleidingsKosten = new List<OpleidingsKost>
            {
                new OpleidingsKost {Id = 1, Beschrijving = "junior java programmer", Bedrag = 1200},
                new OpleidingsKost {Id = 2, Beschrijving = "junior .Net programmer", Bedrag = 1000},
                new OpleidingsKost {Id = 3, Beschrijving = "junior Database Administrator", Bedrag = 2500}
            };
        }

        private void MaakInfrastructuurKosten()
        {
            PersoneelsKosten = new List<PersoneelsKost>
            {
                new PersoneelsKost { Id = 1, Beschrijving = "Toegankelijkheid rolstoel", Bedrag = 5200 },
                new PersoneelsKost { Id = 2, Beschrijving = "Ergonomische bureaustoelen", Bedrag = 10000 },
                new PersoneelsKost { Id = 3, Beschrijving = "test", Bedrag = 2000 }
            };
        }

        private void MaakGereedschapsKosten()
        {
            GereedschapsKosten = new List<GereedschapsKost>
            {
                new GereedschapsKost { Id = 1, Beschrijving = "Overalls", Bedrag = 3000 },
                new GereedschapsKost { Id = 2, Beschrijving = "Werkhandschoenen", Bedrag = 5300 },
                new GereedschapsKost { Id = 3, Beschrijving = "Veiligheidsschoenen", Bedrag = 4000 }
            };
        }

        private void MaakVoorbereidingsKosten()
        {
            VoorbereidingsKosten = new List<VoorbereidingsKost>
            {
                new VoorbereidingsKost { Id = 1, Beschrijving = "test1", Bedrag = 3500 },
                new VoorbereidingsKost { Id = 2, Beschrijving = "test2", Bedrag = 8000 },
                new VoorbereidingsKost { Id = 3, Beschrijving = "test3", Bedrag = 10000 }
            };
        }

        public void MaakEnclaveKosten()
        {
            EnclaveKosten = new List<EnclaveKost>
            {
                new EnclaveKost { Id = 1,  Beschrijving = "test1", Bedrag = 24000 },
                new EnclaveKost { Id = 2,  Beschrijving = "test2", Bedrag = 24000 },
                new EnclaveKost { Id = 3,  Beschrijving = "test3", Bedrag = 24000 }
            };
        }
        #endregion

        #region Methoden baten
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
                    BrutoMaandloonFulltime = 2500
                },
                new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
                {
                    Id = 5,
                    Uren = 37,
                    BrutoMaandloonFulltime = 3500
                },
                new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
                {
                    Id = 6,
                    Uren = 28,
                    BrutoMaandloonFulltime = 2750
                }
            };
        }

        private void MaakSubsidie()
        {
            Subsidie = new Subsidie {Id = 3, Bedrag = 3500};
        }

        private void MaakUitzendKrachtBesparingen()
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

        private void MaakLogistiekeBesparing()
        {
            LogistiekeBesparing = new LogistiekeBesparing
            {
                TransportKosten = 3000,
                LogistiekHandlingsKosten = 2000
            };
        }

        private void MaakExterneInkopen()
        {
            ExterneInkopen = new List<ExterneInkoop>
            {
                new ExterneInkoop {Id = 1, Beschrijving = "test1", Bedrag = 2500},
                new ExterneInkoop {Id = 2, Beschrijving = "test2", Bedrag = 3000},
                new ExterneInkoop {Id = 3, Beschrijving = "test3", Bedrag = 1000}
            };
        }

        private void MaakExtraBesparingen()
        {
            ExtraBesparingen = new List<ExtraBesparing>
            {
                new ExtraBesparing{ Id = 1, Beschrijving = "test1", Bedrag = 3458},
                new ExtraBesparing{ Id = 2, Beschrijving = "test2", Bedrag = 1209},
                new ExtraBesparing{ Id = 3, Beschrijving = "test3", Bedrag = 329}
            };
        }
        #endregion
    }
}
