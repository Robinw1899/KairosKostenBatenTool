using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Baten
{
    public class ExtraBesparingTest
    {
        private ExtraBesparing _extraBesparing;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _extraBesparing = new ExtraBesparing();
            Assert.Equal(Type.Baat, _extraBesparing.Type);
            Assert.Equal(Soort.ExtraBesparing, _extraBesparing.Soort);
        }
    }
}
