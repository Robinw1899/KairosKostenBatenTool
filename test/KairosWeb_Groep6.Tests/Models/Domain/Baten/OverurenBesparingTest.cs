using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Baten
{
    public class OverurenBesparingTest
    {
        private OverurenBesparing _overurenBesparing;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _overurenBesparing = new OverurenBesparing();
            Assert.Equal(Type.Baat, _overurenBesparing.Type);
            Assert.Equal(Soort.OverurenBesparing, _overurenBesparing.Soort);
        }
    }
}
