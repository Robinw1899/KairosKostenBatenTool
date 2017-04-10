using System;
using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Tests.Data;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Extensions
{
    public class LoonkostExtensionsTest
    {
        private readonly List<Loonkost> _loonkosten;

        private readonly int _aantalWerkuren = 37;

        private readonly double _patronaleBijdrage = 35;

        public LoonkostExtensionsTest()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();
            _loonkosten = dbContext.Loonkosten;
        }

        [Fact]
        public void TestGeefTotaalBrutoloonPerMaandAlleLoonkosten_ReturnsBrutoloonAlleFuncties()
        {
            double totaal = LoonkostExtensions
                .GeefTotaalBrutolonenPerJaarAlleLoonkosten(_loonkosten, _aantalWerkuren, _patronaleBijdrage);

            totaal = Math.Round(totaal, 2);
            Assert.Equal(80430.81, totaal);
        }

        [Fact]
        public void TestGeefTotaalAlleLoonkosten_ReturnSumVanTotalenAlleLoonkosten()
        {
            double totaal = LoonkostExtensions
                .GeefTotaalAlleLoonkosten(_loonkosten, _aantalWerkuren, _patronaleBijdrage);

            totaal = Math.Round(totaal, 2);
            Assert.Equal(57837.13, totaal);
        }
    }
}
