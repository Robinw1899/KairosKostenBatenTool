using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IWerkgeverRepository
    {
        IEnumerable<Werkgever> GetAll();
        IEnumerable<Werkgever> GetAllByName(string naam);
        IEnumerable<Werkgever> GetByName(string naam,int beginIndex,int eindIndex);
        IEnumerable<Werkgever> GetWerkgevers(string naam = "", int beginIndex = 0, int eindIndex = 10);      
        Werkgever GetById(int id);
        void Add(Werkgever werkgever);
        void Remove(Werkgever werkgever);
        void Save();
    }
}
