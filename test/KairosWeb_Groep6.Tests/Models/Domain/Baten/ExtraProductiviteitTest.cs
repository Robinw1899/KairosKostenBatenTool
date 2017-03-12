using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Baten
{
    public class ExtraProductiviteitTest
    {
        private ExtraProductiviteit _extraProductiviteit;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _extraProductiviteit = new ExtraProductiviteit();
            Assert.Equal(Type.Baat, _extraProductiviteit.Type);
            Assert.Equal(Soort.ExtraProductiviteit, _extraProductiviteit.Soort);
        }
    }
}
