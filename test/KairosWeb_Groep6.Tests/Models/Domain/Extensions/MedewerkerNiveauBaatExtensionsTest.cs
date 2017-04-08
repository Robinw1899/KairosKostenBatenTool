using System;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Tests.Data;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Extensions
{
    public class MedewerkerNiveauBaatExtensionsTest
    {
        private readonly DummyApplicationDbContext _dbContext;

        private readonly int _aantalWerkuren = 37;

        private readonly double _patronaleBijdrage = 35;

        public MedewerkerNiveauBaatExtensionsTest()
        {
            _dbContext = new DummyApplicationDbContext();
        }

        [Fact]
        public void TestGeefTotaal_MedewerkerNiveauBaat()
        {
            double totaal = MedewerkerNiveauBaatExtensions
                .GeefTotaalBrutolonenPerJaarAlleLoonkosten(_dbContext.MedewerkerNiveauBaten, _aantalWerkuren, _patronaleBijdrage);
            totaal = Math.Round(totaal, 2);

            Assert.Equal(132432.81, totaal);
        }
    }
}
