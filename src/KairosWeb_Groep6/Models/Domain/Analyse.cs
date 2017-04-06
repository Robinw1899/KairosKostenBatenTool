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
        
        public int AnalyseId { get; set; }

       
        public Departement Departement { get; set; }

        
        public DateTime DatumCreatie { get; set; } = DateTime.Now;

        
        // bij aanmaak van analyse ook steeds op vandaag zetten
        public DateTime DatumLaatsteAanpassing { get; set; } = DateTime.Now;
        #endregion

        #region Kosten
        
        public List<Loonkost> Loonkosten { get; set; } = new List<Loonkost>();

        
        public List<EnclaveKost> EnclaveKosten { get; set; } = new List<EnclaveKost>();

        
        public List<VoorbereidingsKost> VoorbereidingsKosten { get; set; } = new List<VoorbereidingsKost>();

        
        public List<InfrastructuurKost> InfrastructuurKosten { get; set; } = new List<InfrastructuurKost>();

        
        public List<GereedschapsKost> GereedschapsKosten { get; set; } = new List<GereedschapsKost>();

        
        public List<OpleidingsKost> OpleidingsKosten { get; set; } = new List<OpleidingsKost>();

        
        public List<BegeleidingsKost> BegeleidingsKosten { get; set; } = new List<BegeleidingsKost>();

        
        public List<ExtraKost> ExtraKosten { get; set; } = new List<ExtraKost>();
        #endregion

        #region Baten
       
        public List<MedewerkerNiveauBaat> MedewerkersZelfdeNiveauBaat { get; set; } = new List<MedewerkerNiveauBaat>();

        
        public List<MedewerkerNiveauBaat> MedewerkersHogerNiveauBaat { get; set; } = new List<MedewerkerNiveauBaat>();

        
        public List<UitzendKrachtBesparing> UitzendKrachtBesparingen { get; set; } = new List<UitzendKrachtBesparing>();

        
        public ExtraOmzet ExtraOmzet { get; set; }

        
        public ExtraProductiviteit ExtraProductiviteit { get; set; }

        
        public OverurenBesparing OverurenBesparing { get; set; }

        
        public List<ExterneInkoop> ExterneInkopen { get; set; } = new List<ExterneInkoop>();

        
        public Subsidie Subsidie { get; set; }

        
        public List<ExtraBesparing> ExtraBesparingen { get; set; } = new List<ExtraBesparing>();
        #endregion

        #region Constructors
        public Analyse()
        {
            
        }
        #endregion

        #region Methods
        public IDictionary<Soort, double> GeefTotalenKosten()
        {
            IDictionary<Soort, double> resultaat = new Dictionary<Soort, double>();
            double totaal;

            // Loonkosten
            if(Departement != null)
            {
                totaal = LoonkostExtensions.GeefTotaalAlleLoonkosten(Loonkosten,
                                                                    Departement.Werkgever.AantalWerkuren,
                                                                    Departement.Werkgever.PatronaleBijdrage);
            }
            else
            {
                totaal = 0;
            }
            
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
            if (Departement != null)
            {
                totaal = BegeleidingsKostExtensions.GeefTotaal(BegeleidingsKosten, Departement.Werkgever.PatronaleBijdrage);
            }
            else
            {
                totaal = 0;
            }

            resultaat.Add(Soort.BegeleidingsKost, totaal);

            // Extra kosten
            totaal = KostOfBaatExtensions.GeefTotaal(ExtraKosten);
            resultaat.Add(Soort.ExtraKost, totaal);

            return resultaat;
        }

        public IDictionary<Soort, double> GeefTotalenBaten()
        {
            IDictionary<Soort, double> resultaat = new Dictionary<Soort, double>();
            double totaal = 0;

            // Medewerkers zelfde niveau
            if (Departement != null)
            {
                totaal = MedewerkerNiveauBaatExtensions.GeefTotaalBrutolonenPerJaarAlleLoonkosten(
                                                        MedewerkersZelfdeNiveauBaat,
                                                        Departement.Werkgever.AantalWerkuren,
                                                        Departement.Werkgever.PatronaleBijdrage);
            }
            else
            {
                totaal = 0;
            }

            resultaat.Add(Soort.MedewerkersZelfdeNiveau, totaal);

            // Medewerkers hoger niveau
            if (Departement != null)
            {
                totaal = MedewerkerNiveauBaatExtensions.GeefTotaalBrutolonenPerJaarAlleLoonkosten(
                                                        MedewerkersHogerNiveauBaat,
                                                        Departement.Werkgever.AantalWerkuren,
                                                        Departement.Werkgever.PatronaleBijdrage);
            }
            else
            {
                totaal = 0;
            }

            resultaat.Add(Soort.MedewerkersHogerNiveau, totaal);

            // Uitzendkrachtbesparingen
            totaal = KostOfBaatExtensions.GeefTotaal(UitzendKrachtBesparingen);
            resultaat.Add(Soort.UitzendkrachtBesparing, totaal);

            // Extra omzet
            if (ExtraOmzet != null)
            {
                totaal = ExtraOmzet.Bedrag;
            }
            else
            {
                totaal = 0;
            }
            
            resultaat.Add(Soort.ExtraOmzet, totaal);

            // Extra productiviteit
            if (ExtraProductiviteit != null)
            {
                totaal = ExtraProductiviteit.Bedrag;
            }
            else
            {
                totaal = 0;
            }
            resultaat.Add(Soort.ExtraProductiviteit, totaal);

            // Overurenbesparing
            if (OverurenBesparing != null)
            {
                totaal = OverurenBesparing.Bedrag;
            }
            else
            {
                totaal = 0;
            }
            resultaat.Add(Soort.OverurenBesparing, totaal);

            // Externe inkopen
            totaal = KostOfBaatExtensions.GeefTotaal(ExterneInkopen);
            resultaat.Add(Soort.ExterneInkoop, totaal);

            // Subsidie
            if(Subsidie != null)
            {
                totaal = Subsidie.Bedrag;
            }
            else
            {
                totaal = 0;
            }
            resultaat.Add(Soort.Subsidie, totaal);

            // Extra besparingen
            totaal = KostOfBaatExtensions.GeefTotaal(ExtraBesparingen);
            resultaat.Add(Soort.ExtraBesparing, totaal);

            return resultaat;
        }
        #endregion
    }
}
