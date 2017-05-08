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
        public IEnumerable<Werkgever> GetAllByName(string naam)
        {
            if (naam.Equals(""))
                return GetAll();
            return _werkgevers
                .Where(w => w.Naam.Contains(naam))                            
                .Include(w => w.Departementen)
                .ToList();
        }

        public IEnumerable<Werkgever> GetByName(string naam,int beginIndex , int eindIndex)
        {
            return _werkgevers
                .Where(w => w.Naam.Contains(naam))
                .Skip(beginIndex)
                .Take(eindIndex - beginIndex)
                .Include(w => w.Departementen)
                .ToList();
                
        }
        public IEnumerable<Werkgever> GetWerkgevers(string naam = "", int beginIndex = 0, int eindIndex = 10)
        {
            //dit zou best in de controller gebeure
            List<Werkgever> legeWerkgevers;
            if (!naam.Equals(""))
            {
                legeWerkgevers = GetByName(naam, beginIndex, eindIndex).ToList();
            }
            else
            {
                legeWerkgevers = _werkgevers
                    .Skip(beginIndex)
                    .Take(eindIndex - beginIndex)
                    .Include(w => w.Departementen)
                    .ToList();

            }

            return legeWerkgevers;

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
