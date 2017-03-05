using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Rij
    {
        public List<object> Waarden { get; private set; }

        public Rij(int grootte)
        {
            Waarden = new List<object>(grootte);
        }
    }
}
