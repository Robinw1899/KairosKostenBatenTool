﻿using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class GebruikerRepository : IGebruikerRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Gebruiker> _gebruikers;

        public GebruikerRepository(ApplicationDbContext _context)
        {
            _dbContext = _context;
            //_jobcoaches = _context.Jobcoaches;
        }
        public IEnumerable<Gebruiker> GetAll()
        {
            return _gebruikers
                .AsNoTracking();
        }

        public Gebruiker GetBy(string email)
        {
            return new Gebruiker("Aelbrecht", "Thomas", email, false);
            //return _gebruikers
              //  .FirstOrDefault(g => g.Emailadres.Equals(email));
        }

        public void Add(Gebruiker gebruiker)
        {
            _gebruikers.Add(gebruiker);
        }

        public void Remove(Gebruiker gebruiker)
        {
            _gebruikers.Remove(gebruiker);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
