using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Baten
{
    class LogistiekeBesparingTest
    {
        private LogistiekeBesparing _logistiekeBesparing;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _logistiekeBesparing = new LogistiekeBesparing();
            Assert.Equal(Type.Baat, _logistiekeBesparing.Type);
            Assert.Equal(Soort.LogistiekeBesparing, _logistiekeBesparing.Soort);
        }
    }
}
