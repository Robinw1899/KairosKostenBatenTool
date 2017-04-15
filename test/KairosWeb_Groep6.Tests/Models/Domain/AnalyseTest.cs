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
                { Soort.Loonkost, 57837.13M }, // In DummyDb
                { Soort.ExtraKost, 1550.00M }, // In DummyDb
                { Soort.BegeleidingsKost, 2437.11M }, // In DummyDb
                { Soort.EnclaveKost, 0 },
                { Soort.VoorbereidingsKost, 0 },
                { Soort.InfrastructuurKost, 0 },
                { Soort.GereedschapsKost, 0 },
                { Soort.OpleidingsKost, 0 }                
            };

            _analyse = new Analyse
            {
                Departement = dbContext.Aldi,
                Loonkosten = dbContext.Loonkosten,
                ExtraKosten = dbContext.ExtraKosten,
                BegeleidingsKosten =  dbContext.BegeleidingsKosten
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

            ICollection<Soort> soorten = totalen.Keys;

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
                { Soort.LoonkostSubsidies, 0},
                { Soort.MedewerkersZelfdeNiveau, 266516.27M }, // In DummyDb
                { Soort.MedewerkersHogerNiveau, 0 }, 
                { Soort.UitzendkrachtBesparing, 17570.00M }, // In DummyDb
                { Soort.ExtraOmzet, 0 },
                { Soort.ExtraProductiviteit, 0 },
                { Soort.OverurenBesparing, 0 },
                { Soort.ExterneInkoop, 0 },
                { Soort.Subsidie, 3500M }, // In DummyDb
                { Soort.LogistiekeBesparing, 5000M },
                { Soort.ExtraBesparing, 0 }
            };

            _analyse = new Analyse
            {
                Departement = dbContext.Aldi,
                MedewerkersZelfdeNiveauBaat = dbContext.MedewerkerNiveauBaten,
                UitzendKrachtBesparingen = dbContext.UitzendKrachtBesparingen,
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
