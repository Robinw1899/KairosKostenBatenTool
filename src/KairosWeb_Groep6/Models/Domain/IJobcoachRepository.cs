using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    interface IJobcoachRepository
    {
        void AddJobcoach(Jobcoach jobcoach);
        void RemoveJobcoach(Jobcoach jobcoach);
        Jobcoach GetById(string email);
        IEnumerable<Jobcoach> GetAll();
        void SaveChanges();
    }
}
