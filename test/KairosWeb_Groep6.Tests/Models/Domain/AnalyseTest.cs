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
            IDictionary<Soort, double> totalen = _analyse.GeefTotalenKosten();

            Assert.False(totalen.Any(t => t.Value > 0)); // er mag geen enkel totaal groter zijn dan 0
        }

        [Fact]
        public void TestGeefTotalenKosten_8SoortenInDictionary()
        {
            _analyse = new Analyse();
            IDictionary<Soort, double> totalen = _analyse.GeefTotalenKosten();

            int aantalSoorten = totalen.Count;

            Assert.False(totalen.Any(t => t.Value > 0)); // er mag geen enkel totaal groter zijn dan 0
            Assert.Equal(8, aantalSoorten);
        }

        [Fact]
        public void TestGeefTotalenKosten()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();

            IDictionary<Soort, double> expected = new Dictionary<Soort, double>
            {
                { Soort.Loonkost, 57837.13 }, // In DummyDb
                { Soort.ExtraKost, 1550.00 }, // In DummyDb
                { Soort.BegeleidingsKost, 2437.11 }, // In DummyDb
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

            IDictionary<Soort, double> totalen = _analyse.GeefTotalenKosten();

            foreach (KeyValuePair<Soort, double> pair in totalen)
            {
                double totaal = Math.Round(pair.Value, 2);
                Assert.Equal(expected[pair.Key], totaal);
            }
        }
        #endregion

        #region Totalen baten
        [Fact]
        public void TestGeefTotalenBaten_LegeAnalyse_Allemaal0()
        {
            _analyse = new Analyse();
            IDictionary<Soort, double> totalen = _analyse.GeefTotalenBaten();

            ICollection<Soort> soorten = totalen.Keys;
            int aantalSoorten = soorten.Distinct().Count();

            Assert.False(totalen.Any(t => t.Value > 0)); // er mag geen enkel totaal groter zijn dan 0
            Assert.Equal(9, aantalSoorten);
        }

        [Fact]
        public void TestGeefTotalenBaten_8SoortenInDictionary()
        {
            _analyse = new Analyse();
            IDictionary<Soort, double> totalen = _analyse.GeefTotalenBaten();

            int aantalSoorten = totalen.Count;

            Assert.Equal(9, aantalSoorten);
        }

        [Fact]
        public void TestGeefTotalenBaten()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();

            IDictionary<Soort, double> expected = new Dictionary<Soort, double>
            {
                { Soort.MedewerkersZelfdeNiveau, 132432.81 }, // In DummyDb
                { Soort.MedewerkersHogerNiveau, 0 }, 
                { Soort.UitzendkrachtBesparing, 17570.00 }, // In DummyDb
                { Soort.ExtraOmzet, 0 },
                { Soort.ExtraProductiviteit, 0 },
                { Soort.OverurenBesparing, 0 },
                { Soort.ExterneInkoop, 0 },
                { Soort.Subsidie, 200 }, // In DummyDb
                { Soort.ExtraBesparing, 0 }
            };

            _analyse = new Analyse
            {
                Departement = dbContext.Aldi,
                MedewerkersZelfdeNiveauBaat = dbContext.MedewerkerNiveauBaten,
                UitzendKrachtBesparingen = dbContext.UitzendKrachtBesparingen,
                Subsidie = dbContext.Subsidies[0]
            };

            IDictionary<Soort, double> totalen = _analyse.GeefTotalenBaten();

            foreach (KeyValuePair<Soort, double> pair in totalen)
            {
                double totaal = Math.Round(pair.Value, 2);
                Assert.Equal(expected[pair.Key], totaal);
            }
        }
        #endregion
    }
}
