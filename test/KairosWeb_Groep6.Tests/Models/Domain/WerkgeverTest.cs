using System;
using KairosWeb_Groep6.Models.Domain;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain
{
    public class WerkgeverTest
    {
        [Fact]
        public void TestConstructorStandaardWaarden()
        {
            double patronaleBijdrage = Math.Round(Werkgever.PatronaleBijdrage, 2);
            Assert.Equal(0.35, patronaleBijdrage);
        }
    }
}
