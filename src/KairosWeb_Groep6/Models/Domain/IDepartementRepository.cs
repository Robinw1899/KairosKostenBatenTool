using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IDepartementRepository
    {
        //dit nog aan te vullen indien we nog extra zoekfuncties willen 
        IEnumerable<Departement> GetAll();
        IEnumerable<Departement> GetByName(string naam);
        Departement GetById(int id);
        void Add(Departement werkgever);
        void Remove(Departement werkgever);
        void Save();
    }
}
