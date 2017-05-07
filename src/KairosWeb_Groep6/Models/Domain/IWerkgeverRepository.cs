using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IWerkgeverRepository
    {
        IEnumerable<Werkgever> GetAll();
        IEnumerable<Werkgever> GetByName(string naam);
        IEnumerable<Werkgever> GetWerkgevers( int beginIndex, int eindIndex);      
        Werkgever GetById(int id);
        void Add(Werkgever werkgever);
        void Remove(Werkgever werkgever);
        void Save();
    }
}
