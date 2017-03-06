using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class WerkgeverRepository : IWerkgeverRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Werkgever> _werkgevers;

        public WerkgeverRepository(ApplicationDbContext _context)
        {
            _dbContext = _context;
            _werkgevers = _context.Werkgevers;
        }
        public IEnumerable<Werkgever> GetAll()
        {
            return _werkgevers.AsNoTracking();
        }

        public Werkgever GetByName(string naam)
        {
            return _werkgevers.SingleOrDefault(w => w.Naam == naam);
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
