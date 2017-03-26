using System;
using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Newtonsoft.Json;
using KairosWeb_Groep6.Models.Domain.Extensions;

namespace KairosWeb_Groep6.Models.Domain
{
    public class Analyse
    {
        #region Andere properties
        [JsonProperty]
        public int AnalyseId { get; set; }

        [JsonProperty]
        public Departement Departement { get; set; }

        [JsonProperty]
        public DateTime DatumCreatie { get; set; } = DateTime.Now;

        [JsonProperty]
        public DateTime DatumLaatsteAanpassing { get; set; }
        #endregion

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
        public IDictionary<Soort, double> GeefResultaat()
        {
            IDictionary<Soort, double> resultaat = new Dictionary<Soort, double>();

            // Totalen van alle kosten in resultaat steken
            ZetTotaalKostenInResultaat(resultaat);

            // Totalen van alle baten in resultaat steken
            ZetTotaalBatenInResultaat(resultaat);

            return resultaat;
        }

        private void ZetTotaalKostenInResultaat(IDictionary<Soort, double> resultaat)
        {
            double totaal = 0;

            // Loonkosten
            totaal = LoonkostExtensions.GeefTotaalAlleLoonkosten(Loonkosten,
                                                                    Departement.Werkgever.AantalWerkuren,
                                                                    Departement.Werkgever.PatronaleBijdrage);
            resultaat.Add(Soort.Loonkost, totaal);

            // Enclavekosten
            totaal = KostOfBaatExtensions.GeefTotaal(EnclaveKosten);
            resultaat.Add(Soort.EnclaveKost, totaal);

            // Voorbereidingskosten
            totaal = KostOfBaatExtensions.GeefTotaal(VoorbereidingsKosten);
            resultaat.Add(Soort.VoorbereidingsKost, totaal);

            // Infrastructuurkosten
            totaal = KostOfBaatExtensions.GeefTotaal(InfrastructuurKosten);
            resultaat.Add(Soort.InfrastructuurKost, totaal);

            // Gereedschapskosten
            totaal = KostOfBaatExtensions.GeefTotaal(GereedschapsKosten);
            resultaat.Add(Soort.GereedschapsKost, totaal);

            // Opleidingskosten
            totaal = KostOfBaatExtensions.GeefTotaal(OpleidingsKosten);
            resultaat.Add(Soort.OpleidingsKost, totaal);

            // Begeleidingskosten
            totaal = BegeleidingsKostExtensions.GeefTotaal(BegeleidingsKosten, Departement.Werkgever.PatronaleBijdrage);
            resultaat.Add(Soort.BegeleidingsKost, totaal);

            // Extra kosten
            totaal = KostOfBaatExtensions.GeefTotaal(ExtraKosten);
            resultaat.Add(Soort.ExtraKost, totaal);
        }

        private void ZetTotaalBatenInResultaat(IDictionary<Soort, double> resultaat)
        {
            double totaal = 0;

            // Medewerkers zelfde niveau
            totaal = MedewerkerNiveauBaatExtensions.GeefTotaalBrutolonenPerJaarAlleLoonkosten(
                                                        MedewerkersZelfdeNiveauBaat,
                                                        Departement.Werkgever.AantalWerkuren,
                                                        Departement.Werkgever.PatronaleBijdrage);
            resultaat.Add(Soort.MedewerkersZelfdeNiveau, totaal);

            // Medewerkers hoger niveau
            totaal = MedewerkerNiveauBaatExtensions.GeefTotaalBrutolonenPerJaarAlleLoonkosten(
                                                        MedewerkersHogerNiveauBaat,
                                                        Departement.Werkgever.AantalWerkuren,
                                                        Departement.Werkgever.PatronaleBijdrage);
            resultaat.Add(Soort.MedewerkersHogerNiveau, totaal);

            // Uitzendkrachtbesparingen
            totaal = KostOfBaatExtensions.GeefTotaal(UitzendKrachtBesparingen);
            resultaat.Add(Soort.UitzendkrachtBesparing, totaal);

            // Extra omzet
            totaal = ExtraOmzet.Bedrag;
            resultaat.Add(Soort.ExtraOmzet, totaal);

            // Extra productiviteit
            totaal = ExtraProductiviteit.Bedrag;
            resultaat.Add(Soort.ExtraProductiviteit, totaal);

            // Overurenbesparing
            totaal = OverurenBesparing.Bedrag;
            resultaat.Add(Soort.OverurenBesparing, totaal);

            // Externe inkopen
            totaal = KostOfBaatExtensions.GeefTotaal(ExterneInkopen);
            resultaat.Add(Soort.ExterneInkoop, totaal);

            // Subsidie
            totaal = Subsidie.Bedrag;
            resultaat.Add(Soort.Subsidie, totaal);

            // Extra besparingen
            totaal = KostOfBaatExtensions.GeefTotaal(ExtraBesparingen);
            resultaat.Add(Soort.ExtraBesparing, totaal);
        }
        #endregion
    }
}
