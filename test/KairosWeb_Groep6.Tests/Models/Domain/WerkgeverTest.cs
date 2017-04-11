using KairosWeb_Groep6.Models.Domain;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain
{
    public class WerkgeverTest
    {
        [Fact]
        public void TestConstructorStandaardWaarden()
        {
            Werkgever werkgever = new Werkgever();

            Assert.Equal(35, werkgever.PatronaleBijdrage);
        }
    }
}
