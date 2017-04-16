using System;
using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Tests.Data;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Extensions
{
    public class BegeleidingsKostExtensionstTest
    {
        private readonly decimal _patronaleBijdrage = 35M;

        [Fact]
        public void TestGeefTotaal_LegeLijst_Returns0()
        {
            List<BegeleidingsKost> begeleidingsKosten = new List<BegeleidingsKost>();

            decimal totaal = BegeleidingsKostExtensions.GeefTotaal(begeleidingsKosten, _patronaleBijdrage);

            Assert.Equal(0, totaal);
        }

        [Fact]
        public void TestGeefTotaal()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();

            List<BegeleidingsKost> begeleidingsKosten = dbContext.BegeleidingsKosten;

            decimal totaal = BegeleidingsKostExtensions.GeefTotaal(begeleidingsKosten, _patronaleBijdrage);
            totaal = Math.Round(totaal, 2);

            Assert.Equal(2437.11M, totaal);
        }
    }
}
