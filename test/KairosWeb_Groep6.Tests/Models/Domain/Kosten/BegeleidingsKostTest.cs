using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Kosten
{
    public class BegeleidingsKostTest
    {
        private BegeleidingsKost _begeleidingsKost;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _begeleidingsKost = new BegeleidingsKost();
            Assert.Equal(Type.Kost, _begeleidingsKost.Type);
            Assert.Equal(Soort.BegeleidingsKost, _begeleidingsKost.Soort);
        }
    }
}
