using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Newtonsoft.Json;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Analyse
    {
        [JsonProperty]
        public int AnalyseId { get; set; }

        [JsonProperty]
        public Departement Departement { get; set; } 
        #region Kosten
        [JsonProperty]
        public List<Loonkost> Loonkosten { get; set; } = new List<Loonkost>();

        [JsonProperty]
        public List<EnclaveKost> EnclaveKosten { get; set; } = new List<EnclaveKost>();

        [JsonProperty]
        public List<VoorbereidingsKost> VoorbereidingsKosten { get; set; } = new List<VoorbereidingsKost>();

        [JsonProperty]
        public List<InfrastructuurKost> InfrastructuurKosten { get; set; } = new List<InfrastructuurKost>();

        [JsonProperty]
        public List<GereedschapsKost> GereedschapsKosten { get; set; } = new List<GereedschapsKost>();

        [JsonProperty]
        public List<OpleidingsKost> OpleidingsKosten { get; set; } = new List<OpleidingsKost>();

        [JsonProperty]
        public List<BegeleidingsKost> BegeleidingsKosten { get; set; } = new List<BegeleidingsKost>();

        [JsonProperty]
        public List<ExtraKost> ExtraKosten { get; set; } = new List<ExtraKost>();
        #endregion

        #region Baten
        [JsonProperty]
        public List<MedewerkerNiveauBaat> MedewerkersZelfdeNiveauBaat { get; set; } = new List<MedewerkerNiveauBaat>();

        [JsonProperty]
        public List<MedewerkerNiveauBaat> MedewerkersHogerNiveauBaat { get; set; } = new List<MedewerkerNiveauBaat>();

        [JsonProperty]
        public List<UitzendKrachtBesparing> UitzendKrachtBesparingen { get; set; } = new List<UitzendKrachtBesparing>();

        [JsonProperty]
        public ExtraOmzet ExtraOmzet { get; set; }

        [JsonProperty]
        public ExtraProductiviteit ExtraProductiviteit { get; set; }

        [JsonProperty]
        public OverurenBesparing OverurenBesparing { get; set; }

        [JsonProperty]
        public List<ExterneInkoop> ExterneInkopen { get; set; } = new List<ExterneInkoop>();

        [JsonProperty]
        public Subsidie Subsidie { get; set; }

        [JsonProperty]
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
