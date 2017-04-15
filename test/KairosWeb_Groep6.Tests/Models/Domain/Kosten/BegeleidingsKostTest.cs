using System;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Xunit;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Tests.Models.Domain.Kosten
{
    public class BegeleidingsKostTest
    {
        private BegeleidingsKost _begeleidingsKost;

        private readonly decimal _patronaleBijdrage = 35M;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _begeleidingsKost = new BegeleidingsKost();
            Assert.Equal(Type.Kost, _begeleidingsKost.Type);
            Assert.Equal(Soort.BegeleidingsKost, _begeleidingsKost.Soort);
        }

        [Fact]
        public void TestSetBedrag_DoetNiets()
        {
            _begeleidingsKost = new BegeleidingsKost
            {
                Bedrag = 0
            };

            Assert.Equal(0, _begeleidingsKost.Bedrag);
        }

        [Theory]
        [InlineData(36, 3000, 959.21)]
        [InlineData(24, 2890, 616.03)]
        [InlineData(40, 2999, 1065.43)]
        [InlineData(15, 1540, 205.16)]
        [InlineData(30, 2650, 706.09)]
        public void TestGeefJaarbedrag_GegevenOntbreekt_Returns0(decimal uren, decimal loon, decimal expected)
        {
            _begeleidingsKost = new BegeleidingsKost
            {
                Uren = uren,
                BrutoMaandloonBegeleider = loon
            };

            decimal jaarbedrag = _begeleidingsKost.GeefJaarbedrag(_patronaleBijdrage);
            jaarbedrag = Math.Round(jaarbedrag, 2);

            Assert.Equal(expected, jaarbedrag);
        }
    }
}
