using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IOrganisatieRepository
    {
        IEnumerable<Organisatie> GetAllByNaam(string naam);
        Organisatie GetById(int id);
        void Add(Organisatie organisatie);
        void Remove(Organisatie organisatie);
        void Save();
    }
}
