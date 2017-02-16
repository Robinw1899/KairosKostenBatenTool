using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Analyse
    {
        public Werkgever Werkgever { get; set; }
        public ICollection<Kost> Kosten { get; private set; }
        public ICollection<Baat> Baten { get; private set; }

        public Analyse()
        {
            Kosten = new List<Kost>();
            Baten = new List<Baat>();
        }
    }
}
