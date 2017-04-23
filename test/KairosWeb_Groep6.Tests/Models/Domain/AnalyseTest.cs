using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Tests.Data;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain
{
    public class AnalyseTest
    {
        private Analyse _analyse;

        #region Totalen kosten
        [Fact]
        public void TestGeefTotalenKosten_LegeAnalyse_Allemaal0()
        {
            _analyse = new Analyse();
            IDictionary<Soort, decimal> totalen = _analyse.GeefTotalenKosten();

            Assert.False(totalen.Any(t => t.Value > 0)); // er mag geen enkel totaal groter zijn dan 0
        }

        [Fact]
        public void TestGeefTotalenKosten_8SoortenInDictionary()
        {
            _analyse = new Analyse();
            IDictionary<Soort, decimal> totalen = _analyse.GeefTotalenKosten();

            int aantalSoorten = totalen.Count;

            Assert.Equal(8, aantalSoorten);
        }

        [Fact]
        public void TestGeefTotalenKosten()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();

            IDictionary<Soort, decimal> expected = new Dictionary<Soort, decimal>
            {
                { Soort.Loonkost, 80430.81M },
                { Soort.ExtraKost, 1550M },
                { Soort.BegeleidingsKost, 2437.11M }, 
                { Soort.EnclaveKost, 72000M },
                { Soort.VoorbereidingsKost, 21500M },
                { Soort.PersoneelsKost, 17200M },
                { Soort.GereedschapsKost, 12300M },
                { Soort.OpleidingsKost, 4700M }                
            };

            _analyse = new Analyse
            {
                Departement = dbContext.Aldi,

                /* KOSTEN */
                Loonkosten = dbContext.Loonkosten,
                ExtraKosten = dbContext.ExtraKosten,
                BegeleidingsKosten = dbContext.BegeleidingsKosten,
                OpleidingsKosten = dbContext.OpleidingsKosten,
                PersoneelsKosten = dbContext.PersoneelsKosten,
                GereedschapsKosten = dbContext.GereedschapsKosten,
                VoorbereidingsKosten = dbContext.VoorbereidingsKosten,
                EnclaveKosten = dbContext.EnclaveKosten
            };

            IDictionary<Soort, decimal> totalen = _analyse.GeefTotalenKosten();

            foreach (KeyValuePair<Soort, decimal> pair in totalen)
            {
                decimal totaal = Math.Round(pair.Value, 2);
                Assert.Equal(expected[pair.Key], totaal);
            }
        }
        #endregion

        #region Totalen baten
        [Fact]
        public void TestGeefTotalenBaten_LegeAnalyse_Allemaal0()
        {
            _analyse = new Analyse();
            IDictionary<Soort, decimal> totalen = _analyse.GeefTotalenBaten();

            Assert.False(totalen.Any(t => t.Value > 0)); // er mag geen enkel totaal groter zijn dan 0
        }

        [Fact]
        public void TestGeefTotalenBaten_11SoortenInDictionary()
        {
            _analyse = new Analyse();
            IDictionary<Soort, decimal> totalen = _analyse.GeefTotalenBaten();

            int aantalSoorten = totalen.Count;

            Assert.Equal(11, aantalSoorten);
        }

        [Fact]
        public void TestGeefTotalenBaten()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();

            IDictionary<Soort, decimal> expected = new Dictionary<Soort, decimal>
            {
                { Soort.LoonkostSubsidies, 22593.68M},
                { Soort.MedewerkersZelfdeNiveau, 266516.27M },
                { Soort.MedewerkersHogerNiveau, 266516.27M }, 
                { Soort.UitzendkrachtBesparing, 17570.00M },
                { Soort.ExtraOmzet, 600M },
                { Soort.ExtraProductiviteit, 6470M },
                { Soort.OverurenBesparing, 34570M },
                { Soort.ExterneInkoop, 6500M },
                { Soort.Subsidie, 3500M },
                { Soort.LogistiekeBesparing, 5000M },
                { Soort.ExtraBesparing, 4996M }
            };

            _analyse = new Analyse
            {
                Departement = dbContext.Aldi,

                /* KOSTEN */
                Loonkosten = dbContext.Loonkosten,
                ExtraKosten = dbContext.ExtraKosten,
                BegeleidingsKosten = dbContext.BegeleidingsKosten,
                OpleidingsKosten = dbContext.OpleidingsKosten,
                PersoneelsKosten = dbContext.PersoneelsKosten,
                GereedschapsKosten = dbContext.GereedschapsKosten,
                VoorbereidingsKosten = dbContext.VoorbereidingsKosten,
                EnclaveKosten = dbContext.EnclaveKosten,

                /* BATEN */
                MedewerkersZelfdeNiveauBaat = dbContext.MedewerkerNiveauBaten,
                MedewerkersHogerNiveauBaat = dbContext.MedewerkerNiveauBaten,
                UitzendKrachtBesparingen = dbContext.UitzendKrachtBesparingen,
                ExterneInkopen = dbContext.ExterneInkopen,
                ExtraOmzet = dbContext.ExtraOmzet,
                ExtraProductiviteit = dbContext.ExtraProductiviteit,
                OverurenBesparing = dbContext.OverurenBesparing,
                Subsidie = dbContext.Subsidie,
                LogistiekeBesparing = dbContext.LogistiekeBesparing,
                ExtraBesparingen = dbContext.ExtraBesparingen
            };

            IDictionary<Soort, decimal> totalen = _analyse.GeefTotalenBaten();

            foreach (KeyValuePair<Soort, decimal> pair in totalen)
            {
                decimal totaal = Math.Round(pair.Value, 2);
                Assert.Equal(expected[pair.Key], totaal);
            }
        }
        #endregion
    }
}
