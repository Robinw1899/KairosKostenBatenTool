using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class DomeinController
    {
        private ICollection<Gebruiker> gebruikers;

        public DomeinController()
        {
            gebruikers = new List<Gebruiker>();
        }
    }
}
