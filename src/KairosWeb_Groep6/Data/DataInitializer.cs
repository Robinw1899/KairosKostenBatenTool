﻿using System.Threading.Tasks;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Identity;

namespace KairosWeb_Groep6.Data
{
    public class DataInitializer
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJobcoachRepository _gebruikerRepository;
        private readonly IWerkgeverRepository _werkgeverRepository;

        public DataInitializer(
            ApplicationDbContext dbContext,
            UserManager<ApplicationUser> userManager, 
            IJobcoachRepository gebruikerRepository,
            IWerkgeverRepository werkgeverRepository)
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
                Werkgever werkgever = new Werkgever("VDAB", "Vooruitgangstraat", 1, 9300, "Aalst", 37);
                _werkgeverRepository.Add(werkgever);

                werkgever = new Werkgever("ALDI", "Leo Duboistraat", 20, 9280, "Lebbeke", 37);
                _werkgeverRepository.Add(werkgever);

                werkgever = new Werkgever("Coolblue", "Medialaan", 1, 1000, "Brussel", 35);
                _werkgeverRepository.Add(werkgever);

                _werkgeverRepository.Save();

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
    }
}
