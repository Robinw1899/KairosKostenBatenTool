using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Kosten
{
    public class InfrastructuurKostTest
    {
        private InfrastructuurKost _infrastructuurKost;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _infrastructuurKost = new InfrastructuurKost();
            Assert.Equal(Type.Kost, _infrastructuurKost.Type);
            Assert.Equal(Soort.InfrastructuurKost, _infrastructuurKost.Soort);
        }
    }
}
