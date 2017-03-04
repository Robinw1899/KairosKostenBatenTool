using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Rij
    {
        private List<Object> waarden;
        public Rij(int grootte)
        {
            waarden = new List<Object>();
        }
    }
}
