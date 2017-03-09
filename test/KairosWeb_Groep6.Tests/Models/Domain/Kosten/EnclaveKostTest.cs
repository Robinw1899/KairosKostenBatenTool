using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Kosten
{
    public class EnclaveKostTest
    {
        private EnclaveKost _enclaveKost;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _enclaveKost = new EnclaveKost();
            Assert.Equal(Type.Kost, _enclaveKost.Type);
            Assert.Equal(Soort.EnclaveKost, _enclaveKost.Soort);
        }
    }
}
