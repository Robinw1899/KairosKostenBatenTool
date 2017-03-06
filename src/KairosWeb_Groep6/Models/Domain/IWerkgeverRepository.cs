using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IWerkgeverRepository
    {
        //dit nog aan te vullen indien we nog extra zoekfuncties willen 
        IEnumerable<Werkgever> GetAll();
        Werkgever GetByName(string naam);      
        void Add(Werkgever werkgever);
        void Remove(Werkgever werkgever);
        void Save();
    }
}
