using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IDepartementRepository
    {
        IEnumerable<Departement> GetAll();
        IEnumerable<Departement> GetAllVanWerkgever(int id);
        Departement GetByName(string name);
        Departement GetById(int id);
        void Add(Departement werkgever);
        void Remove(Departement werkgever);
        void Save();
    }
}
