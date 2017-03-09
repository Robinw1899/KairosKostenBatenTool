using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Baten
{
    public class ExterneInkoopTest
    {
        private ExterneInkoop _externeInkoop;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _externeInkoop = new ExterneInkoop();
            Assert.Equal(Type.Baat, _externeInkoop.Type);
            Assert.Equal(Soort.ExterneInkoop, _externeInkoop.Soort);
        }
    }
}
