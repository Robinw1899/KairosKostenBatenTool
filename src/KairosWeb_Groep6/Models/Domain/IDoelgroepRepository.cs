using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IDoelgroepRepository
    {
        Doelgroep GetByDoelgroepSoort(DoelgroepSoort soort);
        IEnumerable<Doelgroep> GetAll();
        void Add(Doelgroep doelgroep);
        void Remove(Doelgroep doelgroep);
        void Save();
    }
}
