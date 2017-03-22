using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class DepartementRepository : IDepartementRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<Departement> _departementen;

        public DepartementRepository(ApplicationDbContext _context)
        {
            _dbContext = _context;
            _departementen = _context.Departementen;
        }
        public IEnumerable<Departement> GetAll()
        {
            return _departementen
                .Include(d => d.Werkgever)
                .AsNoTracking();
        }

        public IEnumerable<Departement> GetByName(string naam)
        {
            return _departementen
                .Include(d => d.Werkgever)
                .Where(d => d.Werkgever.Naam.Contains(naam))
                .ToList();
        }

        public Departement GetById(int id)
        {
            return _departementen
                .Include(d => d.Werkgever)
                .SingleOrDefault(w => w.DepartementId == id);
        }

        public void Add(Departement werkgever)
        {
            _departementen.Add(werkgever);
        }

        public void Remove(Departement werkgever)
        {
            _departementen.Remove(werkgever);            
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
