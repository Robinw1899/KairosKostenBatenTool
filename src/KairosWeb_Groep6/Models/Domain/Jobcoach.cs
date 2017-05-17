using System.Collections.Generic;
using System.Linq;
using System;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Jobcoach : Persoon
    {
        #region Properties
        public string Wachtwoord { get; set; }

        public bool AlAangemeld { get; set; }

        public Organisatie Organisatie { get; set; }

        public IList<Analyse> Analyses { get; set; } = new List<Analyse>();
        #endregion

        #region Constructors
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
        #endregion

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

        public void SelecteedMatchendeAnalyseDatum(string val)
        {
            if (!string.IsNullOrEmpty(val))
            {
                IList<Analyse> analysesInDatum = null;
                switch (val)
                {
                    case "1":
                        analysesInDatum = Analyses.Where(a => (a.DatumLaatsteAanpassing - DateTime.Today).TotalDays <= 30).ToList();
                        break;
                    case "3":
                        analysesInDatum = Analyses.Where(a => (a.DatumLaatsteAanpassing - DateTime.Today).TotalDays <= 90).ToList();
                        break;
                    case "6":
                        analysesInDatum = Analyses.Where(a => (a.DatumLaatsteAanpassing - DateTime.Today).TotalDays <= 180).ToList();
                        break;
                    case "12":
                        analysesInDatum = Analyses.Where(a => (a.DatumLaatsteAanpassing - DateTime.Today).TotalDays <= 360).ToList();
                        break;
                    case "18":
                        analysesInDatum = Analyses.Where(a => (a.DatumLaatsteAanpassing - DateTime.Today).TotalDays <= 720).ToList();
                        break;

                    default:
                        analysesInDatum = Analyses;
                        break;
                }
                Analyses = analysesInDatum;

            }
        }

        #endregion
    }
}
