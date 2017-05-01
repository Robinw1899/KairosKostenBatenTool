using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IContactPersoonRepository
    {
        IEnumerable<ContactPersoon> GetAll();
        ContactPersoon GetById(int id);
        void Add(ContactPersoon contactPersoon);
        void Remove(ContactPersoon contactPersoon);
        void Save();
    }
}
