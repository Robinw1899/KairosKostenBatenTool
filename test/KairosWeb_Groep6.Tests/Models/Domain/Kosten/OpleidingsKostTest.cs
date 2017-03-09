using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Kosten
{
    public class OpleidingsKostTest
    {
        private OpleidingsKost _opleidingsKost;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _opleidingsKost = new OpleidingsKost();
            Assert.Equal(Type.Kost, _opleidingsKost.Type);
            Assert.Equal(Soort.OpleidingsKost, _opleidingsKost.Soort);
        }
    }
}
