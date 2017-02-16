using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class DomeinController
    {
        private ICollection<Jobcoach> jobcoaches;

        public DomeinController()
        {
            jobcoaches = new List<Jobcoach>();
        }
    }
}
