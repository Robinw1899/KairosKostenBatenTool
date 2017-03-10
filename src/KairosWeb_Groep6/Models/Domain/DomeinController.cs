using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class DomeinController
    {
        private ICollection<Jobcoach> gebruikers;

        public DomeinController()
        {
            gebruikers = new List<Jobcoach>();
        }
    }
}
