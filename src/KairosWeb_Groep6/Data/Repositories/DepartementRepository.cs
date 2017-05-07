using System;
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

        public DepartementRepository(ApplicationDbContext context)
        {
            _dbContext = context;
            _departementen = context.Departementen;
        }

        public IEnumerable<Departement> GetAll()
        {
            return _departementen
                .Include(d => d.Werkgever)
                .Include(d => d.ContactPersoon)
                .AsNoTracking()
                .ToList();
        }

        public IEnumerable<Departement> GetAllVanWerkgever(int id)
        {
            return _departementen
                .Include(d => d.Werkgever)
                .Include(d => d.ContactPersoon)
                .Where(d => d.Werkgever != null)
                .Where(d => d.Werkgever.WerkgeverId == id)
                .ToList();
        }

        public Departement GetByName(string name)
        {
            return _departementen
                .Include(d => d.Werkgever)
                .Include(d => d.ContactPersoon)
                .FirstOrDefault(d => d.Naam.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public Departement GetById(int id)
        {
            return _departementen
                .Include(d => d.Werkgever)
                .Include(d => d.ContactPersoon)
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
