using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

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
                .Include(w => w.Departementen)
                .Where(w => w.Naam.Contains(naam))
                .ToList();
        }
        
        public void Add(Werkgever werkgever)
        {
            _werkgevers.Add(werkgever);
        }

        public void Remove(Werkgever werkgever)
        {
            _werkgevers.Remove(werkgever);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public IEnumerable<Werkgever> GetWerkgevers( int index, int aantal)
        {
            List<Werkgever> legeWerkgevers = _werkgevers              
               .Skip(index)
               .Take(aantal)
               .ToList();

            List<Werkgever> werkgevers = new List<Werkgever>();

            foreach (Werkgever a in legeWerkgevers)
            {
                werkgevers.Add(GetById(a.WerkgeverId));
            }

            return werkgevers;
        }
    }
}
