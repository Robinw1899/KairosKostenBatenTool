using System.Threading.Tasks;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Data
{
    public class DataInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobcoachRepository _gebruikerRepository;
        private readonly IDepartementRepository _departementRepository;
        private readonly IAnalyseRepository _analyseRepository;
        private readonly IWerkgeverRepository _werkgeverRepository;
        private readonly IIntroductietekstRepository _introductietekstRepository;
        private readonly IDoelgroepRepository _doelgroepRepository;

        public DataInitializer(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager, 
            IJobcoachRepository gebruikerRepository,
            IDepartementRepository departementRepository,
            IAnalyseRepository analyseRepository,
            IWerkgeverRepository werkgeverRepository,
            IIntroductietekstRepository introductietekstRepository,
            IDoelgroepRepository doelgroepRepository)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _gebruikerRepository = gebruikerRepository;
            _departementRepository = departementRepository;
            _analyseRepository = analyseRepository;
            _werkgeverRepository = werkgeverRepository;
            _introductietekstRepository = introductietekstRepository;
            _doelgroepRepository = doelgroepRepository;
        }

        public async Task InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                await InitializeUsers();
                // Doelgroepen aanmaken
                InitializeDoelgroepen();

                ContactPersoon contactThomas = new ContactPersoon("Thomas", "Aelbrecht", "thomasaelbrecht@live.com");
                ContactPersoon contactRobin = new ContactPersoon("Robin", "Coppens", "robin.coppens.w1899@student.hogent.be") ;
                ContactPersoon contactDimi = new ContactPersoon("Dimmy", "Maenhout", "dimmy.maenhout@telenet.be");

                Werkgever werkgever =
                    new Werkgever("VDAB", "Vooruitgangstraat", 1, "", 9300, "Aalst", 37);

                Departement departement = new Departement("Onderhoudsdienst") { Werkgever = werkgever };
                werkgever.Departementen.Add(departement);
                _departementRepository.Add(departement);
                _werkgeverRepository.Add(werkgever);

                werkgever =
                    new Werkgever("ALDI", "Leo Duboistraat", 20, "", 9280, "Lebbeke", 37);

                departement = new Departement("Aankoop") { Werkgever = werkgever };
                werkgever.Departementen.Add(departement);
                _departementRepository.Add(departement);
                _werkgeverRepository.Add(werkgever);

                werkgever =
                    new Werkgever("Coolblue", "Medialaan", 1, "", 1000, "Brussel", 35);

                departement = new Departement("Human resources") { Werkgever = werkgever };
                werkgever.Departementen.Add(departement);
                _departementRepository.Add(departement);
                _werkgeverRepository.Add(werkgever);

                werkgever =
                   new Werkgever("Coolblue", "Medialaan", 2, "", 1000, "Brussel", 35);
                _werkgeverRepository.Add(werkgever);
                werkgever =
                   new Werkgever("Coolblue", "Medialaan", 3, "", 1000, "Brussel", 35);
                _werkgeverRepository.Add(werkgever);
                werkgever =
                   new Werkgever("Coolblue", "Medialaan", 4, "", 1000, "Brussel", 35);
                _werkgeverRepository.Add(werkgever);
                werkgever =
                   new Werkgever("Coolblue", "Medialaan", 5, "", 1000, "Brussel", 35);
                _werkgeverRepository.Add(werkgever);
                werkgever =
                   new Werkgever("Coolblue", "Medialaan", 6, "", 1000, "Brussel", 35);
                _werkgeverRepository.Add(werkgever);
                werkgever =
                   new Werkgever("Coolblue", "Medialaan", 7, "", 1000, "Brussel", 35);
                _werkgeverRepository.Add(werkgever);
                werkgever =
                   new Werkgever("Coolblue", "Medialaan", 8, "", 1000, "Brussel", 35);
                _werkgeverRepository.Add(werkgever);
                werkgever =
                   new Werkgever("Coolblue", "Medialaan", 9, "", 1000, "Brussel", 35);
                _werkgeverRepository.Add(werkgever);


                InitializeIntrotekst();
              
                _departementRepository.Save();
                _werkgeverRepository.Save();

                Jobcoach thomas = _gebruikerRepository.GetByEmail("thomasaelbrecht@live.com");

                Analyse analyse = new Analyse();

                departement = new Departement("Verkoop") { Werkgever = werkgever };
                analyse.Departement = departement ;

                analyse.Loonkosten = MaakLoonkosten();

                analyse.ExtraKosten = MaakExtraKosten();

                analyse.MedewerkersZelfdeNiveauBaten = MaakMedewerkerNiveauBaten();

                analyse.Subsidie = new Subsidie { Bedrag = 1500 };

                analyse.UitzendKrachtBesparingen = MaakUitzendKrachtBesparingen();

                _analyseRepository.Add(analyse);
                _analyseRepository.Save();

                thomas.Analyses.Add(analyse);
                _gebruikerRepository.Save();
            }
            _dbContext.SaveChanges();
        }

        public void InitializeDoelgroepen()
        {
            Doelgroep doelgroep = new Doelgroep("Wn's < 25 jaar laaggeschoold", 2500M, 1550M);
            _doelgroepRepository.Add(doelgroep);

            doelgroep = new Doelgroep("Wn's < 25 jaar middengeschoold", 2500M, 1000M);
            _doelgroepRepository.Add(doelgroep);

            doelgroep = new Doelgroep("Wn's ≥ 55 en < 60 jaar", 4466.66M, 1150M);
            _doelgroepRepository.Add(doelgroep);

            doelgroep = new Doelgroep("Wns ≥ 60 jaar", 4466.66M, 1500M);
            _doelgroepRepository.Add(doelgroep);

            doelgroep = new Doelgroep("Andere", 0M, 0M);
            _doelgroepRepository.Add(doelgroep);

            _doelgroepRepository.Save();
        }

        public void InitializeIntrotekst()
        {
            Introductietekst tekst = new Introductietekst
            {
                Titel = "Introductie",
                Vraag = "Wat is Kairos?"
            };

            tekst.Paragrafen.Add(new Paragraaf
            {
                Volgnummer = 1,
                Tekst = "Waarom zou een werkgever investeren in het tewerkstellen van personen met een " +
                        "grote afstand tot de arbeidsmarkt en/of functiecreatie? We zijn geneigd te denken " +
                        "dat we - ook managers en bedrijfsleiders - erg rationele wezens zijn, die (grote) " +
                        "beslissingen weloverwogen maken. Maar... zijn menselijke beslissingen wel zo " +
                        "rationeel als we denken?"
            });

            tekst.Paragrafen.Add(new Paragraaf
            {
                Volgnummer = 2,
                Tekst = "Niet echt. Mensen beslissen op basis van eigen kennis, ervaring, inschattingsvermogen " +
                        "en intuïtie. We gaan er vanuit dat we daarmee in staat zijn om consistente en evenwichtige " +
                        "beslissingen te nemen of, anders gezegd, een rationele afweging te maken. De waarheid is " +
                        "echter heel anders. Veel van onze besluitvorming wordt door een automatische piloot-functie " +
                        "van onze hersenen gedaan. Met als gevolg dat wij, en ook werkgevers, veel beslissingen nemen" +
                        " op basis van vooroordelen in plaats van alle beschikbare informatie in overweging te nemen."
            });

            tekst.Paragrafen.Add(new Paragraaf
            {
                Volgnummer = 3,
                Tekst = "Het maken van een business case kan helpen om de besluitvorming te verbeteren, want het " +
                        "geeft betrouwbare, op feiten gebaseerde informatie en vermindert de subjectiviteit in " +
                        "beoordeling en besluitvorming.  De kans dat de werkgever op jouw aanbod zal ingaan, vergroot " +
                        "naarmate je deze vraag duidelijk en zakelijk onderbouwd weet te beantwoorden."
            });

            tekst.Paragrafen.Add(new Paragraaf
            {
                Volgnummer = 4,
                Tekst = "Met dit model kan je werkgevers inzicht geven in de economische waarde van het werken " +
                        "met mensen met een grote afstand tot de arbeidsmarkt."
            });

            _introductietekstRepository.Add(tekst);
            _introductietekstRepository.Save();
        }

        private async Task InitializeUsers()
        {
            string voornaam = "Thomas";
            string naam = "Aelbrecht";
            string email = "thomasaelbrecht@live.com";

            ApplicationUser user = new ApplicationUser
            {
                UserName = email,
                Naam = naam,
                Voornaam = voornaam,
                Email = email
            };

            Organisatie organisatie = new Organisatie("HoGent", "Arbeidstraat", 14, "",  9300, "Aalst");
            Jobcoach jobcoach = new Jobcoach(naam, voornaam, email, organisatie) { AlAangemeld = true};
            _gebruikerRepository.Add(jobcoach);

            await _userManager.CreateAsync(user, "kairos2017");

            voornaam = "Robin";
            naam = "Coppens";
            email = "robin.coppens.w1899@student.hogent.be";

            user = new ApplicationUser
            {
                UserName = email,
                Naam = naam,
                Voornaam = voornaam,
                Email = email
            };

            jobcoach = new Jobcoach(naam, voornaam, email) { AlAangemeld = true};
            _gebruikerRepository.Add(jobcoach);

            await _userManager.CreateAsync(user, "kairos2017");

            voornaam = "Dimmy";
            naam = "Maenhout";
            email = "dimmy.maenhout@telenet.be";

            user = new ApplicationUser
            {
                UserName = email,
                Naam = naam,
                Voornaam = voornaam,
                Email = email
            };

            jobcoach = new Jobcoach(naam, voornaam, email) { AlAangemeld = true };
            _gebruikerRepository.Add(jobcoach);

            await _userManager.CreateAsync(user, "kairos2017");

            _gebruikerRepository.Save();
        }

        private List<Loonkost> MaakLoonkosten()
        {
            Loonkost poetsvrouw = new Loonkost
            {
                Beschrijving = "Poetsvrouw",
                BrutoMaandloonFulltime = 1800,
                AantalUrenPerWeek = 37,
                Doelgroep = _doelgroepRepository.GetById(1),
                Ondersteuningspremie = 20M,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0M
            };

            Loonkost secretaresse = new Loonkost
            {
                Beschrijving = "Secretaresse",
                BrutoMaandloonFulltime = 2200,
                AantalUrenPerWeek = 23,
                Doelgroep = _doelgroepRepository.GetById(2),
                Ondersteuningspremie = 20M,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0M
            };

            Loonkost postbode = new Loonkost
            {
                Beschrijving = "Postbode",
                BrutoMaandloonFulltime = 1900,
                AantalUrenPerWeek = 35,
                Doelgroep = _doelgroepRepository.GetById(3),
                Ondersteuningspremie = 20M,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0M
            };

            return new List<Loonkost>
            {
                poetsvrouw,
                secretaresse,
                postbode
            };
        }

        private List<ExtraKost> MaakExtraKosten()
        {
            List<ExtraKost> extraKosten = new List<ExtraKost>
            {
                new ExtraKost { Bedrag = 150, Beschrijving = "Stagekosten"},
                new ExtraKost { Bedrag = 1000, Beschrijving = "Uitrusting"},
                new ExtraKost { Bedrag = 400, Beschrijving = "Boeken en ander studiemateriaal"}
            };
            return extraKosten;
        }

        private List<MedewerkerNiveauBaat> MaakMedewerkerNiveauBaten()
        {
            List<MedewerkerNiveauBaat> medewerkerNiveauBaten = new List<MedewerkerNiveauBaat>
            {
                new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
                {
                    Uren = 35,
                    BrutoMaandloonFulltime = 2300
                },
                new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
                {
                    Uren = 30,
                    BrutoMaandloonFulltime = 2000
                },
                new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
                {
                    Uren = 37,
                    BrutoMaandloonFulltime = 3250
                }
            };
            return medewerkerNiveauBaten;
        }

        public List<UitzendKrachtBesparing> MaakUitzendKrachtBesparingen()
        {
            List<UitzendKrachtBesparing> baten = new List<UitzendKrachtBesparing>
            {
                new UitzendKrachtBesparing() { Beschrijving = "Tuinier", Bedrag = 2500},
                new UitzendKrachtBesparing() { Beschrijving = "Klusjesman", Bedrag = 3500},
                new UitzendKrachtBesparing() { Beschrijving = "WC-madam", Bedrag = 2750}
            };

            return baten;
        }
    }
}
