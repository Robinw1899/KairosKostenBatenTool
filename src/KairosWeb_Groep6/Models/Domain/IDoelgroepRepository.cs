using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IDoelgroepRepository
    {
        Doelgroep GetById(int id);
        //Doelgroep GetByOmschrijving(string omschrijving);
        IEnumerable<Doelgroep> GetAll();
        void Add(Doelgroep doelgroep);
        void Remove(Doelgroep doelgroep);
        void Save();
    }
}
