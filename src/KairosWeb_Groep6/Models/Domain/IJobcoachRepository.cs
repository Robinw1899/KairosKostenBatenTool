using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
