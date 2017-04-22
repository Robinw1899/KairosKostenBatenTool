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

                Loonkosten = dbContext.Loonkosten,
                ExtraKosten = dbContext.ExtraKosten,
                BegeleidingsKosten = dbContext.BegeleidingsKosten,
                MedewerkersZelfdeNiveauBaat = dbContext.MedewerkerNiveauBaten,
                UitzendKrachtBesparingen = dbContext.UitzendKrachtBesparingen,
                ExterneInkopen = dbContext.ExterneInkopen,
                OpleidingsKosten = dbContext.OpleidingsKosten,
                PersoneelsKosten = dbContext.PersoneelsKosten,
                GereedschapsKosten = dbContext.GereedschapsKosten,
                VoorbereidingsKosten = dbContext.VoorbereidingsKosten,
                EnclaveKosten = dbContext.EnclaveKosten,
                Subsidie = dbContext.Subsidie,
                LogistiekeBesparing = dbContext.LogistiekeBesparing
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
                { Soort.LoonkostSubsidies, 22593.68M},// In DummyDb
                { Soort.MedewerkersZelfdeNiveau, 266516.27M }, // In DummyDb
                { Soort.MedewerkersHogerNiveau, 0 }, 
                { Soort.UitzendkrachtBesparing, 17570.00M }, // In DummyDb
                { Soort.ExtraOmzet, 0 },
                { Soort.ExtraProductiviteit, 0 },
                { Soort.OverurenBesparing, 0 },
                { Soort.ExterneInkoop, 6500M },// In DummyDb
                { Soort.Subsidie, 3500M }, // In DummyDb
                { Soort.LogistiekeBesparing, 5000M },// In DummyDb
                { Soort.ExtraBesparing, 0 }
            };

            _analyse = new Analyse
            {
                Departement = dbContext.Aldi,

                Loonkosten = dbContext.Loonkosten,
                ExtraKosten = dbContext.ExtraKosten,
                BegeleidingsKosten = dbContext.BegeleidingsKosten,
                MedewerkersZelfdeNiveauBaat = dbContext.MedewerkerNiveauBaten,
                UitzendKrachtBesparingen = dbContext.UitzendKrachtBesparingen,
                ExterneInkopen = dbContext.ExterneInkopen,
                OpleidingsKosten = dbContext.OpleidingsKosten,
                PersoneelsKosten = dbContext.PersoneelsKosten,
                GereedschapsKosten = dbContext.GereedschapsKosten,
                VoorbereidingsKosten = dbContext.VoorbereidingsKosten,
                EnclaveKosten = dbContext.EnclaveKosten,
                Subsidie = dbContext.Subsidie,
                LogistiekeBesparing = dbContext.LogistiekeBesparing
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
