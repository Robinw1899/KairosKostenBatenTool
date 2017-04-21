using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Kosten
{
    public class PersoneelsKostTest
    {
        private PersoneelsKost _infrastructuurKost;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _infrastructuurKost = new PersoneelsKost();
            Assert.Equal(Type.Kost, _infrastructuurKost.Type);
            Assert.Equal(Soort.PersoneelsKost, _infrastructuurKost.Soort);
        }
    }
}
