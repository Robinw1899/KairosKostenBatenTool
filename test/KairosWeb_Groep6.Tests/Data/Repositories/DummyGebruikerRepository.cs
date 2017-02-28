using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Tests.Data.Repositories
{
    public class DummyGebruikerRepository : IGebruikerRepository
    {
        private readonly DummyApplicationDbContext _dbContext;
        private readonly DbSet<Gebruiker> _gebruikers;
        public IEnumerable<Gebruiker> GetAll()
        {
            return _gebruikers
                .AsNoTracking();
        }

        public Gebruiker GetBy(string email)
        {
            //return new Gebruiker("Aelbrecht", "Thomas", email, false);
            return _gebruikers
              .FirstOrDefault(g => g.Emailadres.Equals(email));
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

        public DummyGebruikerRepository(DummyApplicationDbContext context)
        {
            this._dbContext = context;
        }
    }
}
