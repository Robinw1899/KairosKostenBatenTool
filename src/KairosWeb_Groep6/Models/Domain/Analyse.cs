    using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Analyse
    {
        public Werkgever Werkgever { get; set; }
        public KostenBatenBeheer kostenBatenBeheer { get; set; }

        public Analyse()
        {          
        }
    }
}
