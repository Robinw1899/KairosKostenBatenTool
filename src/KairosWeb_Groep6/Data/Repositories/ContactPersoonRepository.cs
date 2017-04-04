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

        public ContactPersoonRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
          //  _contactPersonen = dbContext.ContactPersonen;
        }
        public void Add(ContactPersoon contactPersoon)
        {
            _contactPersonen.Add(contactPersoon);
        }

        public IEnumerable<ContactPersoon> GetAll()
        {
            throw new NotImplementedException();
        }

        public ContactPersoon GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContactPersoon> GetByName(string naam)
        {
            throw new NotImplementedException();
        }

        public void Remove(ContactPersoon contactPersoon)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
