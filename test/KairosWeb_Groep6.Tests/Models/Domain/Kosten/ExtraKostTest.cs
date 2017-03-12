using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Kosten
{
    public class ExtraKostTest
    {
        private ExtraKost _extraKost;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _extraKost = new ExtraKost();
            Assert.Equal(Type.Kost, _extraKost.Type);
            Assert.Equal(Soort.ExtraKost, _extraKost.Soort);
        }
    }
}
