using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IJobcoachRepository
    {
        IEnumerable<Jobcoach> GetAll();
        Jobcoach GetByEmail(string email);
        Jobcoach GetById(int id);
        List<Departement> GetDepartementenVanJobcoach(Jobcoach jobcoach);
        List<Werkgever> GetWerkgeversVanJobcoach(Jobcoach jobcoach);
        void Add(Jobcoach gebruiker);
        void Remove(Jobcoach gebruiker);
        void Save();
    }
}
