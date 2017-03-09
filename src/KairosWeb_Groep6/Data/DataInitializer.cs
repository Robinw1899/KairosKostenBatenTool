using System.Threading.Tasks;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace KairosWeb_Groep6.Data
{
    public class DataInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IGebruikerRepository _gebruikerRepository;
        private readonly IWerkgeverRepository _werkgeverRepository;

        public DataInitializer(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IGebruikerRepository gebruikerRepository,IWerkgeverRepository werkgeverRepository)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _gebruikerRepository = gebruikerRepository;
            _werkgeverRepository = werkgeverRepository;
        }

        public async Task InitializeData()
        {
            _dbContext.Database.EnsureDeleted();
            if (_dbContext.Database.EnsureCreated())
            {
                await InitializeUsers();
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

            Organisatie organisatie = new Organisatie("HoGent", "Arbeidstraat", 14, 9300, "Aalst");
            Gebruiker gebruiker = new Gebruiker(naam, voornaam, email, organisatie) {AlAangemeld = true, Wachtwoord = "kairos2017" };
            _gebruikerRepository.Add(gebruiker);

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

            gebruiker = new Gebruiker(naam, voornaam, email, false) { AlAangemeld = true, Wachtwoord = "kairos2017" };
            _gebruikerRepository.Add(gebruiker);

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

            gebruiker = new Gebruiker(naam, voornaam, email, false) { AlAangemeld = true, Wachtwoord = "kairos2017" };
            _gebruikerRepository.Add(gebruiker);

            await _userManager.CreateAsync(user, "kairos2017");

            _gebruikerRepository.Save();

            string naamOrg = "VAB";
            string straat = "Capucienelaan";
            int nummer = 65;
            int postcode = 9300;
            string gemeente = "Aalst";
            int aantalWerkuren = 35;
            double patronaleBijdrage = 0;
            
            Werkgever werkgever
                = new Werkgever(naamOrg,straat,nummer,postcode,gemeente,aantalWerkuren,patronaleBijdrage);
             _werkgeverRepository.Add(werkgever);

            naamOrg = "VAB";
            straat = "Capucienelaan";
            nummer = 65;
            postcode = 9300;
            gemeente = "Aalst";
            aantalWerkuren = 35;
            patronaleBijdrage = 0;

            werkgever 
                = new Werkgever(naamOrg, straat, nummer, postcode, gemeente, aantalWerkuren, patronaleBijdrage);
            _werkgeverRepository.Add(werkgever);

            _werkgeverRepository.Save();
        }
    }
}
