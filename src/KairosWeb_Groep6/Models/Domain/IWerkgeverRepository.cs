using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IWerkgeverRepository
    {
        //dit nog aan te vullen indien we nog extra zoekfuncties willen 
        IEnumerable<Werkgever> GetAll();
        IEnumerable<Werkgever> GetByName(string naam);
        Werkgever GetById(int id);
        void Add(Werkgever werkgever);
        void Remove(Werkgever werkgever);
        void Save();
    }
}
