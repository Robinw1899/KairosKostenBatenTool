using System;
using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Tests.Data;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Extensions
{
    public class LoonkostExtensionsTest
    {
        private readonly List<Loonkost> _loonkosten;

        public LoonkostExtensionsTest()
        {
            Werkgever.AantalWerkuren = 37;
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();
            _loonkosten = dbContext.Loonkosten;
        }

        [Fact]
        public void TestGeefTotaalBrutoloonPerMaandAlleLoonkosten_ReturnsBrutoloonAlleFuncties()
        {
            double totaal = _loonkosten.GeefTotaalBrutolonenPerJaarAlleLoonkosten();
            totaal = Math.Round(totaal, 2);
            Assert.Equal(80430.81, totaal);
        }
    }
}
