using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class WerkgeverRepository : IWerkgeverRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Werkgever> _werkgevers;

        public WerkgeverRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
              _werkgevers = dbContext.Werkgevers;
        }
        public void Add(Werkgever werkgever)
        {
            _werkgevers.Add(werkgever);
        }

        public IEnumerable<Werkgever> GetAll()
        {
            return _werkgevers
                .Include(w=>w.Departementen)
                .AsNoTracking();
        }

        public Werkgever GetById(int id)
        {
            return _werkgevers
                .Where(w => w.WerkgeverId == id)
                .Include(w=>w.Departementen)
                .First();
        }

        public IEnumerable<Werkgever> GetByName(string naam)
        {
            return _werkgevers
                .Where(w => w.Naam.Contains(naam))
                .Include(w=>w.Departementen)
                .ToList();
        }

        public void Remove(Werkgever werkgever)
        {
            _werkgevers.Remove(werkgever);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
