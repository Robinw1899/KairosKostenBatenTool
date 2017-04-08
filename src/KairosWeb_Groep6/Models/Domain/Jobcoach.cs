using System.Collections.Generic;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Jobcoach : Persoon
    {
        #region Properties
        public bool AlAangemeld { get; set; }

        public Organisatie Organisatie { get; set; }

        public ICollection<Analyse> Analyses { get; set; } = new List<Analyse>();
        #endregion

        public Jobcoach()
        {
            
        }

        public Jobcoach(string naam, string voornaam, string emailadres)
            : base(voornaam, naam, emailadres)
        {
            AlAangemeld = false;
        }

        public Jobcoach(string naam, string voornaam, string emailadres, Organisatie organisatie)
            : this(naam, voornaam, emailadres)
        {
            Organisatie = organisatie;
            Analyses = new List<Analyse>();
        }
    }
}
