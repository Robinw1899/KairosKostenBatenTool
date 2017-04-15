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

        private readonly decimal _patronaleBijdrage = 35M;

        public MedewerkerNiveauBaatExtensionsTest()
        {
            _dbContext = new DummyApplicationDbContext();
        }

        [Fact]
        public void TestGeefTotaal_MedewerkerNiveauBaat()
        {
            decimal totaal = MedewerkerNiveauBaatExtensions
                .GeefTotaal(_dbContext.MedewerkerNiveauBaten, _aantalWerkuren, _patronaleBijdrage);
            totaal = Math.Round(totaal, 2);

            Assert.Equal(266516.27M, totaal);
        }
    }
}
