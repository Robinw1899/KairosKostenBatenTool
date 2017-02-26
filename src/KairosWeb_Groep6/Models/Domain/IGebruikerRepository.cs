using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IGebruikerRepository
    {
        IEnumerable<Gebruiker> GetAll();
        Gebruiker GetByEmail(string email);
        Gebruiker GetById(int id);
        void Add(Gebruiker gebruiker);
        void Remove(Gebruiker gebruiker);
        void Save();
    }
}
