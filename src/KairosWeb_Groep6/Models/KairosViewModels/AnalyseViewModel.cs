using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;
using System.Linq;

namespace KairosWeb_Groep6.Models.KairosViewModels
{
    public class AnalyseViewModel
    {
        #region Properties
        public int AnalyseId { get; set; }

        public string Departement { get; set; }

        public string Werkgever { get; set; }

        public double KostenTotaal { get; set; }

        public double BatenTotaal { get; set; }

        public double NettoResultaat { get; set; }

        public string KlasseTotaal { get; set; } // klasse die de kleur van het nettores aangeeft

        public KostenIndexViewModel KostenInfo { get; set; }

        public BatenIndexViewModel BatenInfo { get; set; }
        #endregion

        #region Constructor
        public AnalyseViewModel()
        {

        }

        public AnalyseViewModel(Analyse analyse)
        {
            AnalyseId = analyse.AnalyseId;

            if(analyse.Departement != null)
            {
                Departement = analyse.Departement.Naam;
                Werkgever = analyse.Departement.Werkgever.Naam;
            }
            else
            {
                Werkgever = "Werkgever nog niet ingevuld";
                Departement = "Departement nog niet ingevuld";
            }

            KostenInfo = new KostenIndexViewModel(analyse);
            BatenInfo = new BatenIndexViewModel(analyse);

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
            else if (NettoResultaat == 0)
            {
                KlasseTotaal = "alert-warning";
            }
            else
            {
                KlasseTotaal = "alert-success";
            }
        }
        #endregion
    }
}