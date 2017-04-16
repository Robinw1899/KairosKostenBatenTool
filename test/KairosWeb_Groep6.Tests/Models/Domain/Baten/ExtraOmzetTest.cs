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
        [InlineData(0, 0, 0.00)]
        [InlineData(3000, 23, 690.00)]
        [InlineData(1000, 98, 980.00)]
        public void TestBedrag_BerekentTotaal
            (decimal jaarbedragOmzetverlies, decimal besparing, decimal expected)
        {
            _extraOmzet = new ExtraOmzet
            {
                JaarbedragOmzetverlies = jaarbedragOmzetverlies,
                Besparing = besparing
            };

            decimal result = Math.Round(_extraOmzet.Bedrag, 2);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void TestSetBedrag_DoetNiets()
        {
            _extraOmzet = new ExtraOmzet
            {
                Bedrag = 2500
            };

            Assert.Equal(0, _extraOmzet.Bedrag);
        }
    }
}
