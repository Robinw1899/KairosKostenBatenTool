using System;
using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Tests.Data;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Extensions
{
    public class MedewerkerNiveauBaatExtensionsTest
    {
        private readonly DummyApplicationDbContext _dbContext;

        public MedewerkerNiveauBaatExtensionsTest()
        {
            _dbContext = new DummyApplicationDbContext();
        }

        [Fact]
        public void TestGeefTotaal_MedewerkerNiveauBaat()
        {
            double totaal = MedewerkerNiveauBaatExtensions.GeefTotaalBrutolonenPerJaarAlleLoonkosten(_dbContext.MedewerkerNiveauBaten, 37, 35);
            totaal = Math.Round(totaal, 2);

            Assert.Equal(132432.81, totaal);
        }
    }
}
