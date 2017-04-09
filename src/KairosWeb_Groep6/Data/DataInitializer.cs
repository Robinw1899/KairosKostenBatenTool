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
        private readonly IIntroductietekstRepository _introductietekstRepository;

        public DataInitializer(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager, 
            IJobcoachRepository gebruikerRepository,
            IDepartementRepository werkgeverRepository,
            IAnalyseRepository analyseRepository,
            IIntroductietekstRepository introductietekstRepository)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _gebruikerRepository = gebruikerRepository;
            _departementRepository = werkgeverRepository;
            _analyseRepository = analyseRepository;
            _introductietekstRepository = introductietekstRepository;
        }

        public async Task InitializeData()
        {
            //_dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                await InitializeUsers();

                InitializeIntrotekst();

                Werkgever werkgever = new Werkgever("VDAB", "Vooruitgangstraat", 1, "", 9300, "Aalst", 37);
                _departementRepository.Add(new Departement("Onderhoudsdienst") {Werkgever = werkgever});

                werkgever = new Werkgever("ALDI", "Leo Duboistraat", 20, "", 9280, "Lebbeke", 37);
                _departementRepository.Add(new Departement("Aankoop") { Werkgever = werkgever });

                werkgever = new Werkgever("Coolblue", "Medialaan", 1, "", 1000, "Brussel", 35);
                _departementRepository.Add(new Departement("Human resources") { Werkgever = werkgever });

                _departementRepository.Save();

                Jobcoach thomas = _gebruikerRepository.GetByEmail("thomasaelbrecht@live.com");

                Analyse analyse = new Analyse();

                analyse.Departement = new Departement("Verkoop") { Werkgever = werkgever };

                analyse.Loonkosten = MaakLoonkosten();

                analyse.ExtraKosten = MaakExtraKosten();

                analyse.MedewerkersZelfdeNiveauBaat = MaakMedewerkerNiveauBaten();

                analyse.Subsidie = new Subsidie { Id = 2, Bedrag = 1500 };

                analyse.UitzendKrachtBesparingen = MaakUitzendKrachtBesparingen();

                _analyseRepository.Add(analyse);
                _analyseRepository.Save();

                thomas.Analyses.Add(analyse);
                _gebruikerRepository.Save();
            }
            _dbContext.SaveChanges();
        }

        private void InitializeIntrotekst()
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
                Id = 1,
                Beschrijving = "Poetsvrouw",
                BrutoMaandloonFulltime = 1800,
                AantalUrenPerWeek = 37,
                Doelgroep = Doelgroep.LaaggeschooldTot25,
                Ondersteuningspremie = 20D,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0D
            };

            Loonkost secretaresse = new Loonkost
            {
                Id = 2,
                Beschrijving = "Secretaresse",
                BrutoMaandloonFulltime = 2200,
                AantalUrenPerWeek = 23,
                Doelgroep = Doelgroep.MiddengeschooldTot25,
                Ondersteuningspremie = 20D,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0D
            };

            Loonkost postbode = new Loonkost
            {
                Id = 3,
                Beschrijving = "Postbode",
                BrutoMaandloonFulltime = 1900,
                AantalUrenPerWeek = 35,
                Doelgroep = Doelgroep.Tussen55En60,
                Ondersteuningspremie = 20D,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0D
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
            List<ExtraKost>  extraKosten = new List<ExtraKost>();
            extraKosten.Add(new ExtraKost { Id = 1, Bedrag = 150, Beschrijving = "Stagekosten" });
            extraKosten.Add(new ExtraKost { Id = 2, Bedrag = 1000, Beschrijving = "Uitrusting" });
            extraKosten.Add(new ExtraKost { Id = 3, Bedrag = 400, Beschrijving = "Boeken en ander studiemateriaal" });
            return extraKosten;
        }

        private List<MedewerkerNiveauBaat> MaakMedewerkerNiveauBaten()
        {
            List<MedewerkerNiveauBaat>  medewerkerNiveauBaten = new List<MedewerkerNiveauBaat>();
            medewerkerNiveauBaten.Add(new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau) { Id = 1, Uren = 35, BrutoMaandloonFulltime = 2300 });
            medewerkerNiveauBaten.Add(new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau) { Id = 2, Uren = 30, BrutoMaandloonFulltime = 2000 });
            medewerkerNiveauBaten.Add(new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau) { Id = 3, Uren = 37, BrutoMaandloonFulltime = 3250 });
            return medewerkerNiveauBaten;
        }

        public List<UitzendKrachtBesparing> MaakUitzendKrachtBesparingen()
        {
            List<UitzendKrachtBesparing> baten = new List<UitzendKrachtBesparing>
            {
                new UitzendKrachtBesparing() {Id = 1, Beschrijving = "Tuinier", Bedrag = 2500},
                new UitzendKrachtBesparing() {Id = 2, Beschrijving = "Klusjesman", Bedrag = 3500},
                new UitzendKrachtBesparing() {Id = 3, Beschrijving = "WC-madam", Bedrag = 2750}
            };

            return baten;
        }
    }
}
