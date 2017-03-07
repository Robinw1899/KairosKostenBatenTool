using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Baten
{
    public class SubsidieTest
    {
        private Subsidie _subsidie;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _subsidie = new Subsidie();
            Assert.Equal(Type.Baat, _subsidie.Type);
            Assert.Equal(Soort.Subsidie, _subsidie.Soort);
        }
    }
}
