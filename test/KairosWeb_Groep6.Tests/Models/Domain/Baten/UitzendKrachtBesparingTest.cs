using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Baten
{
    public class UitzendKrachtBesparingTest
    {
        private UitzendKrachtBesparing _uitzendKrachtBesparing;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _uitzendKrachtBesparing = new UitzendKrachtBesparing();
            Assert.Equal(Type.Baat, _uitzendKrachtBesparing.Type);
            Assert.Equal(Soort.UitzendkrachtBesparing, _uitzendKrachtBesparing.Soort);
        }
    }
}
