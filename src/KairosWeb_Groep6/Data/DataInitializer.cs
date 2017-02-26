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

        public DataInitializer(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IGebruikerRepository gebruikerRepository)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _gebruikerRepository = gebruikerRepository;
        }

        public async Task InitializeData()
        {
            await InitializeUsers();
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
            Gebruiker gebruiker = new Gebruiker(naam, voornaam, email, organisatie) {AlAangemeld = true};
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

            gebruiker = new Gebruiker(naam, voornaam, email, false) { AlAangemeld = true };
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

            gebruiker = new Gebruiker(naam, voornaam, email, false) { AlAangemeld = true };
            _gebruikerRepository.Add(gebruiker);

            await _userManager.CreateAsync(user, "kairos2017");

            _gebruikerRepository.Save();
        }
    }
}
