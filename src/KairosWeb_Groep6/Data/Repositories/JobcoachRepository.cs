using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class JobcoachRepository : IJobcoachRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Jobcoach> _jobcoaches;

        public JobcoachRepository(ApplicationDbContext _context)
        {
            _dbContext = _context;
            _jobcoaches = _context.Jobcoaches;
        }
        public IEnumerable<Jobcoach> GetAll()
        {
            return _jobcoaches
                .Include(g => g.Organisatie)
                .Include(g => g.Analyses)
                .AsNoTracking();
        }

        public Jobcoach GetByEmail(string email)
        {
            return _jobcoaches
                .Include(g => g.Organisatie)
                .Include(g => g.Analyses)
                    .ThenInclude(g => g.Departement)
                            .ThenInclude(d => d.Werkgever)
                .SingleOrDefault(g => g.Emailadres.Equals(email));
        }

        public Jobcoach GetById(int id)
        {
            return _jobcoaches
                .Include(g => g.Organisatie)
                .Include(g => g.Analyses)
                    .ThenInclude(g => g.Departement)
                        .ThenInclude(d => d.Werkgever)
                .SingleOrDefault(g => g.PersoonId == id);
        }

        public void Add(Jobcoach gebruiker)
        {
            _jobcoaches.Add(gebruiker);
        }

        public void Remove(Jobcoach gebruiker)
        {
            _jobcoaches.Remove(gebruiker);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
