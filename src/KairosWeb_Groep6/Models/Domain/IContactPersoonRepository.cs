using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IContactPersoonRepository
    {
        IEnumerable<ContactPersoon> GetAll();
        IEnumerable<ContactPersoon> GetByName(string naam);
        ContactPersoon GetById(int id);
        void Add(ContactPersoon contactPersoon);
        void Remove(ContactPersoon contactPersoon);
        void Save();
    }
}
