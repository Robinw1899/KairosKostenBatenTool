using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class ContactPersoonRepository : IContactPersoonRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<ContactPersoon> _contactPersonen;

        public ContactPersoonRepository(ApplicationDbContext context)
        {
            _dbContext = context;
            _contactPersonen = context.ContactPersonen;
        }
        public void Add(ContactPersoon contactPersoon)
        {
           _contactPersonen.Add(contactPersoon);
        }

        public IEnumerable<ContactPersoon> GetAll()
        {
            return _contactPersonen
                 .AsNoTracking()
                 .ToList();
        }

        public ContactPersoon GetById(int id)
        {
            return _contactPersonen
                .SingleOrDefault(c => c.ContactPersoonId == id);
        }

        public ContactPersoon GetByName(string naam)
        {
            return _contactPersonen
                .FirstOrDefault(d => d.Naam.Equals(naam, StringComparison.OrdinalIgnoreCase));
        }

        public void Remove(ContactPersoon contactPersoon)
        {
            _contactPersonen.Remove(contactPersoon);
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
