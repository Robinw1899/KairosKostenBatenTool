using KairosWeb_Groep6.Models.Domain;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class AnalyseViewModel
    {
        #region Properties
        public int AnalyseId { get; set; }

        public string Departement { get; set; }

        public string Werkgever { get; set; }

        public string Gemeente { get; set; }

        public decimal KostenTotaal { get; set; }

        public decimal BatenTotaal { get; set; }

        public decimal NettoResultaat { get; set; }

        public bool InArchief { get; set; }

        public decimal KostenPercent { get; set; }

        public decimal BatenPercent { get; set; }

        public string KlasseTotaal { get; set; } // klasse die de kleur van het nettores aangeeft
        #endregion

        #region Constructors
        public AnalyseViewModel()
        {

        }

        public AnalyseViewModel(Analyse analyse)
        {
            AnalyseId = analyse.AnalyseId;

            InArchief = analyse.InArchief;

            if(analyse.Departement != null)
            {
                Departement = analyse.Departement.Naam;
                Werkgever = analyse.Departement.Werkgever.Naam;
                Gemeente = analyse.Departement.Werkgever.Gemeente;
            }
            else
            {
                Werkgever = "Onbekend";
                Departement = "Onbekend";
            }

            KostenTotaal = analyse.KostenTotaal;

            BatenTotaal = analyse.BatenTotaal;

            NettoResultaat = BatenTotaal - KostenTotaal;

            if (BatenTotaal != 0 || KostenTotaal != 0)
            {
                BatenPercent = (BatenTotaal / (BatenTotaal + KostenTotaal)) * 100;
                KostenPercent = (KostenTotaal / (BatenTotaal + KostenTotaal)) * 100;
            }
            else
            {
                BatenPercent = 0;
                KostenPercent = 0;
            }

            // kleur voor nettoresultaat bepalen
            if (NettoResultaat < 0)
            {
                KlasseTotaal = "alert-danger";
            }
            else if (NettoResultaat > 0)
            {
                KlasseTotaal = "alert-success";
            }
            else
            {
                KlasseTotaal = "alert-warning";
            }
        }
        #endregion
    }
}