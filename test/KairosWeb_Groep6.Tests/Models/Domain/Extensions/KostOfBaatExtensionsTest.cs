using System;
using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Tests.Data;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Extensions
{
    public class KostOfBaatExtensionsTest
    {
        private readonly DummyApplicationDbContext _dbContext;

        public KostOfBaatExtensionsTest()
        {
            _dbContext = new DummyApplicationDbContext();
        }

        [Fact]
        public void TestGetBy_GeenGevonden_ReturnsNull()
        {
            Loonkost loonkost = KostOfBaatExtensions.GetBy(_dbContext.Loonkosten, 0);
            Assert.Null(loonkost);
        }

        #region Loonkosten
        [Fact]
        public void TestGetBy_Loonkosten()
        {
            Loonkost loonkost = KostOfBaatExtensions.GetBy(_dbContext.Loonkosten, 2);

            Assert.Equal(_dbContext.Secretaresse, loonkost);
        }

        [Fact]
        public void TestGeefTotaalThrowsException_Loonkosten()
        {
            Assert.Throws<InvalidOperationException>(() => KostOfBaatExtensions.GeefTotaal(_dbContext.Loonkosten));
        }
        #endregion

        #region ExtraKosten 
        [Fact]
        public void TestGetBy_ExtaKosten()
        {
            ExtraKost extrakost = KostOfBaatExtensions.GetBy(_dbContext.ExtraKosten, 3);

            Assert.Equal(400, extrakost.Bedrag);
            Assert.Equal("Boeken en ander studiemateriaal", extrakost.Beschrijving);
        }

        [Fact]
        public void TestGeefTotaal_ExtaKosten()
        {
            List<KostOfBaat>  extraKosten = new List<KostOfBaat>(_dbContext.ExtraKosten);

            // totaal van alle loonkosten:
            double totaal = KostOfBaatExtensions.GeefTotaal(extraKosten);
            totaal = Math.Round(totaal, 2);
 
            Assert.Equal(1550.00, totaal);
        }
        #endregion

        #region MedewerkerNiveauBaat
        [Fact]
        public void TestGetBy_MedewerkerNiveauBaat()
        {
            MedewerkerNiveauBaat baat = KostOfBaatExtensions.GetBy(_dbContext.MedewerkerNiveauBaten, 1);

            Assert.Equal(2300, baat.BrutoMaandloonFulltime);
            Assert.Equal(35, baat.Uren);
        }

        [Fact]
        public void TestGeefTotaalThrowsException_MedewerkerNiveauBaat()
        {
            Assert.Throws<InvalidOperationException>(() => KostOfBaatExtensions.GeefTotaal(_dbContext.MedewerkerNiveauBaten));
        }
        #endregion

        #region Subsidie
        [Fact]
        public void TestGetBy_Subsidie()
        {
            Subsidie subsidie = KostOfBaatExtensions.GetBy(_dbContext.Subsidies, 2);

            Assert.Equal(1500, subsidie.Bedrag);
        }

        [Fact]
        public void TestGeefTotaal_Subsidie()
        {
            double totaal = KostOfBaatExtensions.GeefTotaal(_dbContext.Subsidies);
            totaal = Math.Round(totaal, 2);

            Assert.Equal(26950.00, totaal);
        }
        #endregion

        #region UitzendkrachtBesparingen
        [Fact]
        public void TestGetBy_UitzendkrachtBesparingen()
        {
            UitzendKrachtBesparing baat = KostOfBaatExtensions.GetBy(_dbContext.UitzendKrachtBesparingen, 4);

            Assert.Equal(5400, baat.Bedrag);
        }

        [Fact]
        public void TestGeefTotaal_UitzendkrachtBesparingen()
        {
            double totaal = KostOfBaatExtensions.GeefTotaal(_dbContext.UitzendKrachtBesparingen);
            totaal = Math.Round(totaal, 2);

            Assert.Equal(17570.00, totaal);
        }
        #endregion

        #region BegeleidingsKosten
        [Fact]
        public void TestGetBy_BegeleidingsKosten()
        {
            BegeleidingsKost kost = KostOfBaatExtensions.GetBy(_dbContext.BegeleidingsKosten, 2);

            Assert.Equal(25, kost.Uren);
            Assert.Equal(2500, kost.BrutoMaandloonBegeleider);
        }

        [Fact]
        public void TestGeefTotaalThrowsException_BegeleidingsKosten()
        {
            Assert.Throws<InvalidOperationException>(() => KostOfBaatExtensions.GeefTotaal(_dbContext.BegeleidingsKosten));
        }
        #endregion
    }
}
