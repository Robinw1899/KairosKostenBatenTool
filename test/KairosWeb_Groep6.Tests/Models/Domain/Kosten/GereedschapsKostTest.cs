using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Kosten
{
    public class GereedschapsKostTest
    {
        private GereedschapsKost _gereedschapsKost;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _gereedschapsKost = new GereedschapsKost();
            Assert.Equal(Type.Kost, _gereedschapsKost.Type);
            Assert.Equal(Soort.GereedschapsKost, _gereedschapsKost.Soort);
        }
    }
}
