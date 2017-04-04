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

        public DataInitializer(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager, 
            IJobcoachRepository gebruikerRepository,
            IDepartementRepository werkgeverRepository,
            IAnalyseRepository analyseRepository)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _gebruikerRepository = gebruikerRepository;
            _departementRepository = werkgeverRepository;
            _analyseRepository = analyseRepository;
        }

        public async Task InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                await InitializeUsers();
                ContactPersoon contactThomas = new ContactPersoon("Thomas", "Aelbrecht", "thomasaelbrecht@live.com");
                ContactPersoon contactRobin = new ContactPersoon("Robin", "Coppens", "robin.coppens.w1899@student.hogent.be");
                ContactPersoon contactDimi = new ContactPersoon("Dimmy", "Maenhout", "dimmy.maenhout@telenet.be");

                List<ContactPersoon> contacten = new List<ContactPersoon>();

                contacten.Add(contactThomas);
                contacten.Add(contactRobin);
                contacten.Add(contactDimi);

                Werkgever werkgever = new Werkgever("VDAB", "Vooruitgangstraat", 1, 9300, "Aalst", 37);
                werkgever.ContactPersonen = contacten;
                werkgever.HoofdContactPersoon = contactThomas;
                _departementRepository.Add(new Departement("Onderhoudsdienst") {Werkgever = werkgever});
              

                werkgever = new Werkgever("ALDI", "Leo Duboistraat", 20, 9280, "Lebbeke", 37);
                werkgever.ContactPersonen = contacten;
                werkgever.HoofdContactPersoon = contactRobin;
                _departementRepository.Add(new Departement("Aankoop") { Werkgever = werkgever });
            

                werkgever = new Werkgever("Coolblue", "Medialaan", 1, 1000, "Brussel", 35);
                werkgever.ContactPersonen = contacten;
                werkgever.HoofdContactPersoon = contactDimi;
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
            Jobcoach jobcoach = new Jobcoach(naam, voornaam, email, organisatie) { AlAangemeld = true, Wachtwoord = "kairos2017" };
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

            jobcoach = new Jobcoach(naam, voornaam, email) { AlAangemeld = true, Wachtwoord = "kairos2017" };
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

            jobcoach = new Jobcoach(naam, voornaam, email) { AlAangemeld = true, Wachtwoord = "kairos2017" };
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
