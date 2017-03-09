using System;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Xunit;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Tests.Models.Domain.Baten
{
    public class ExtraOmzetTest
    {
        private ExtraOmzet _extraOmzet;

        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _extraOmzet = new ExtraOmzet();
            Assert.Equal(Type.Baat, _extraOmzet.Type);
            Assert.Equal(Soort.ExtraOmzet, _extraOmzet.Soort);
        }

        [Theory]
        [InlineData(0, 0, 200, 0.00)]
        [InlineData(3000, 23, 530, 690.00)]
        [InlineData(1000, 98, 4325, 980.00)]
        public void TestBedrag_SetBedragDoetNiets
            (double jaarbedragOmzetverlies, double besparing, double bedrag, double expected)
        {
            _extraOmzet = new ExtraOmzet
            {
                JaarbedragOmzetverlies = jaarbedragOmzetverlies,
                Besparing = besparing,
                Bedrag = bedrag
            };
            // andere bedragen waarmee Bedrag wordt berekend, zijn ook 0
            // daarom is Bedrag sowieso 0

            double result = Math.Round(_extraOmzet.Bedrag, 2);
            Assert.Equal(expected, result);
        }
    }
}
