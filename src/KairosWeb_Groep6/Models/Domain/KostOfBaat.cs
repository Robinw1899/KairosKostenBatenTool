using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.Domain
{
    interface KostOfBaat
    {
        double getBedrag(int rijNr);
        double berekenTotaal();
        Type type { get; set; }
        ICollection<KolomWaarde> kolommen {get;set;}
        ICollection<Rij> waarden { get;set; }

    }
}
