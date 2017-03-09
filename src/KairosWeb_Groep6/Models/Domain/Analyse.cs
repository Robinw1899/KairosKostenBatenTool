using System.Collections.Generic;
using System.Net.Http.Headers;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Kosten;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Analyse
    {
        public int Id { get; set; }

        public Werkgever Werkgever { get; set; }

        #region Kosten
        public List<Loonkost> Loonkosten { get; set; } = new List<Loonkost>();

        public List<EnclaveKost> EnclaveKosten { get; set; } = new List<EnclaveKost>();

        public List<VoorbereidingsKost> VoorbereidingsKosten { get; set; } = new List<VoorbereidingsKost>();

        public List<InfrastructuurKost> InfrastructuurKosten { get; set; } = new List<InfrastructuurKost>();

        public List<GereedschapsKost> GereedschapsKosten { get; set; } = new List<GereedschapsKost>();

        public List<OpleidingsKost> OpleidingsKosten { get; set; } = new List<OpleidingsKost>();

        public List<BegeleidingsKost> BegeleidingsKosten { get; set; } = new List<BegeleidingsKost>();
        #endregion

        #region Baten
        public List<MedewerkerNiveauBaat> MedewerkersZelfdeNiveauBaat { get; set; } = new List<MedewerkerNiveauBaat>();

        public List<MedewerkerNiveauBaat> MedewerkersHogerNiveauBaat { get; set; } = new List<MedewerkerNiveauBaat>();

        public List<UitzendKrachtBesparing> UitzendKrachtBesparingen { get; set; } = new List<UitzendKrachtBesparing>();

        public ExtraOmzet ExtraOmzet { get; set; }

        public ExtraProductiviteit ExtraProductiviteit { get; set; }

        public OverurenBesparing OverurenBesparing { get; set; }

        public List<ExterneInkoop> ExterneInkopen { get; set; } = new List<ExterneInkoop>();

        public List<Subsidie> Subsidies { get; set; } = new List<Subsidie>();

        public List<ExtraBesparing> ExtraBesparingen { get; set; } = new List<ExtraBesparing>();
        #endregion

        #region Constructors
        public Analyse()
        {
            
        }
        #endregion

        #region Methods

        #endregion
    }
}
