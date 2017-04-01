using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class JobcoachRepository : IJobcoachRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Jobcoach> _gebruikers;

        public JobcoachRepository(ApplicationDbContext _context)
        {
            _dbContext = _context;
            _gebruikers = _context.Gebruikers;
        }
        public IEnumerable<Jobcoach> GetAll()
        {
            return _gebruikers
                .Include(g => g.Organisatie)
                .Include(g => g.Analyses)
                .AsNoTracking();
        }

        public Jobcoach GetByEmail(string email)
        {
            return _gebruikers
                .Include(g => g.Organisatie)
                .Include(g => g.Analyses)
                .SingleOrDefault(g => g.Emailadres.Equals(email));
        }

        public Jobcoach GetById(int id)
        {
            return _gebruikers
                .Include(g => g.Organisatie)
                .Include(g => g.Analyses)
                .SingleOrDefault(g => g.PersoonId == id);
        }

        public void Add(Jobcoach gebruiker)
        {
            _gebruikers.Add(gebruiker);
        }

        public void Remove(Jobcoach gebruiker)
        {
            _gebruikers.Remove(gebruiker);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
