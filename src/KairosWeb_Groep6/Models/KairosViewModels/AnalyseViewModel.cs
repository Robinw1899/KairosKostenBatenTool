using KairosWeb_Groep6.Models.Domain;
using System.Linq;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class AnalyseViewModel
    {
        #region Properties
        public int AnalyseId { get; set; }

        public string Departement { get; set; }

        public string Werkgever { get; set; }

        public string Gemeente { get; set; }

        public double KostenTotaal { get; set; }

        public double BatenTotaal { get; set; }

        public double NettoResultaat { get; set; }

        public bool InArchief { get; set; }

        public string KlasseTotaal { get; set; } // klasse die de kleur van het nettores aangeeft
        #endregion

        #region Constructor
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

            KostenTotaal =  analyse.GeefTotalenKosten()
                                     .Sum(x => x.Value);

            BatenTotaal = analyse.GeefTotalenBaten()
                                    .Sum(x => x.Value);

            NettoResultaat = BatenTotaal - KostenTotaal;

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