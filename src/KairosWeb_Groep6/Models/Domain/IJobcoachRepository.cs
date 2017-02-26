using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public interface IJobcoachRepository
    {
        void AddJobcoach(Jobcoach jobcoach);
        void RemoveJobcoach(Jobcoach jobcoach);
        Jobcoach GetById(int id);
        Jobcoach GetByEmail(string email);
        IEnumerable<Jobcoach> GetAll();
        void SaveChanges();
    }
}
