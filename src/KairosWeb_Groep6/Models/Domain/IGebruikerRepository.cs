using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IGebruikerRepository
    {
        IEnumerable<Gebruiker> GetAll();
        Gebruiker GetBy(string email);
        void Add(Gebruiker gebruiker);
        void Remove(Gebruiker gebruiker);
        void Save();
    }
}
