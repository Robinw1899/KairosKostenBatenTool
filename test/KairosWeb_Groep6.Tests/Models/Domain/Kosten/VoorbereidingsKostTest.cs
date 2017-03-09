using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Kosten
{
    public class VoorbereidingsKostTest
    {
        private VoorbereidingsKost _voorbereidingsKost;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _voorbereidingsKost = new VoorbereidingsKost();
            Assert.Equal(Type.Kost, _voorbereidingsKost.Type);
            Assert.Equal(Soort.VoorbereidingsKost, _voorbereidingsKost.Soort);
        }
    }
}
