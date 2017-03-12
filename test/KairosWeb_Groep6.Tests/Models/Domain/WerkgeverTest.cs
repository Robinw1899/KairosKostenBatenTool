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
            Assert.Equal(35, Werkgever.PatronaleBijdrage);
        }
    }
}
