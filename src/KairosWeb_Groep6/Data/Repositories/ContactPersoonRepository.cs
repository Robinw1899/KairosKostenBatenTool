using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace KairosWeb_Groep6.Data.Repositories
{
    public class ContactPersoonRepository : IContactPersoonRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<ContactPersoon> _contactPersonen;

        public ContactPersoonRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _contactPersonen = dbContext.ContactPersonen;
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
                .FirstOrDefault(c => c.ContactPersoonId == id);
        }

        public void Add(ContactPersoon contactPersoon)
        {
            _contactPersonen.Add(contactPersoon);
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
