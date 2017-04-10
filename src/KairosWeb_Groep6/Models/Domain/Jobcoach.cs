using System.Collections.Generic;
using System.Linq;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Jobcoach : Persoon
    {
        #region Properties
        public bool AlAangemeld { get; set; }

        public Organisatie Organisatie { get; set; }

        public IList<Analyse> Analyses { get; set; } = new List<Analyse>();
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

        #region Methods

        public void SelecteerMatchendeAnalyse(string zoekterm)
        {
            if (!string.IsNullOrEmpty(zoekterm))
            {
                List<Analyse> analysesDepartementen = Analyses
                    .Where(t => t.Departement != null)
                    .Where(t => t.Departement.Contains(zoekterm))
                    .ToList();

                List<Analyse> analysesWerkgever = Analyses
                    .Where(t => t.Departement != null)
                    .Where(t => t.Departement.Werkgever.Contains(zoekterm))
                    .ToList();

                // alles in één lijst steken
                List<Analyse> analyses = new List<Analyse>(analysesDepartementen);
                analyses.AddRange(analysesWerkgever);

                Analyses = analyses;
            }
        }
        #endregion
    }
}
