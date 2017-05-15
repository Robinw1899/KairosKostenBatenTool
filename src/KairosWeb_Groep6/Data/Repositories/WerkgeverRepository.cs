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
        public IEnumerable<Werkgever> GetByName(Jobcoach jobcoach, string naam)
        {
            return jobcoach
                .Analyses
                .Where(a => a.Departement != null)
                .Select(a => a.Departement)
                .Select(d => d.Werkgever)
                .Distinct()
                .Where(w => w.Naam.Contains(naam))
                .ToList();

        }
       
        public IEnumerable<Werkgever> GetWerkgevers()
        {                                     
            return _werkgevers
                .Skip(0)
                .Take(10)
                .Include(w => w.Departementen)
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

      
    }
}
