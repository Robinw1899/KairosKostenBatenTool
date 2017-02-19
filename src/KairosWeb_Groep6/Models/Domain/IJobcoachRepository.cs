using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    interface IJobcoachRepository
    {
        void AddJobcoach(Jobcoach jobcoach);
        void RemoveJobcoach(Jobcoach jobcoach);
        Jobcoach GetById(int id);
        IEnumerable<Jobcoach> GetAll();
        void SaveChanges();
    }
}
