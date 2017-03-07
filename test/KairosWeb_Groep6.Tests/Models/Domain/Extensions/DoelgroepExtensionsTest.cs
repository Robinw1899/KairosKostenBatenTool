using System;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.Domain.Extensions;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Extensions
{
    public class DoelgroepExtensionsTest
    {
        #region Gegevens ontbreken
        [Fact]
        public void TestBerekenDoelgroepVermindering_GegevensOntbreken()
        {
            // Werkgever.AantalWerkuren en brutoloon ontbreken
            Doelgroep doelgroep = Doelgroep.LaaggeschooldTot25;
            Assert.Throws<InvalidOperationException>(() => doelgroep.BerekenDoelgroepVermindering(0, 30));
        }
        #endregion

        #region LaaggeschooldTot25
        [Theory]
        [InlineData(37, Doelgroep.LaaggeschooldTot25, 1800, 37, 387.5)]
        [InlineData(37, Doelgroep.LaaggeschooldTot25, 2450, 23, 240.88)]
        [InlineData(37, Doelgroep.LaaggeschooldTot25, 1500, 35, 366.55)]
        public void TestBerekenDoelgroepVermindering_LaaggeschooldTot25_BrutoloonKleinerDanMinBrutoloon
            (int werkuren, Doelgroep doelgroep, double brutoloon, double urenPerWeek, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            double doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, Doelgroep.LaaggeschooldTot25, 2650, 37, 0)]
        [InlineData(37, Doelgroep.LaaggeschooldTot25, 3000, 23, 0)]
        [InlineData(37, Doelgroep.LaaggeschooldTot25, 5000, 35, 0)]
        public void TestBerekenDoelgroepVermindering_LaaggeschooldTot25_BrutoloonGroterDanMinBrutoloon
            (int werkuren, Doelgroep doelgroep, double brutoloon, double urenPerWeek, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            double doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, Doelgroep.LaaggeschooldTot25, 2500, 37, 0)]
        [InlineData(37, Doelgroep.LaaggeschooldTot25, 2500, 23, 0)]
        [InlineData(37, Doelgroep.LaaggeschooldTot25, 2500, 35, 0)]
        public void TestBerekenDoelgroepVermindering_LaaggeschooldTot25_BrutoloonGelijkAanMinBrutoloon
            (int werkuren, Doelgroep doelgroep, double brutoloon, double urenPerWeek, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            double doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion

        #region MiddengeschooldTot25
        [Theory]
        [InlineData(37, Doelgroep.MiddengeschooldTot25, 1800, 37, 250.00)]
        [InlineData(37, Doelgroep.MiddengeschooldTot25, 2200, 23, 155.41)]
        [InlineData(37, Doelgroep.MiddengeschooldTot25, 1900, 35, 236.49)]
        public void TestBerekenDoelgroepVermindering_MiddengeschooldTot25_BrutoloonKleinerDanMinBrutoloon
            (int werkuren, Doelgroep doelgroep, double brutoloon, double urenPerWeek, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            double doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, Doelgroep.MiddengeschooldTot25, 2890, 37, 0)]
        [InlineData(37, Doelgroep.MiddengeschooldTot25, 3100, 23, 0)]
        [InlineData(37, Doelgroep.MiddengeschooldTot25, 2756, 35, 0)]
        public void TestBerekenDoelgroepVermindering_MiddengeschooldTot25_BrutoloonGroterDanMinBrutoloon
            (int werkuren, Doelgroep doelgroep, double brutoloon, double urenPerWeek, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            double doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, Doelgroep.MiddengeschooldTot25, 2500, 37, 0)]
        [InlineData(37, Doelgroep.MiddengeschooldTot25, 2500, 23, 0)]
        [InlineData(37, Doelgroep.MiddengeschooldTot25, 2500, 35, 0)]
        public void TestBerekenDoelgroepVermindering_MiddengeschooldTot25_BrutoloonGelijkAanMinBrutoloon
            (int werkuren, Doelgroep doelgroep, double brutoloon, double urenPerWeek, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            double doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion

        #region Tussen55En60
        [Theory]
        [InlineData(37, Doelgroep.Tussen55En60, 2378, 37, 287.50)]
        [InlineData(37, Doelgroep.Tussen55En60, 3850, 23, 178.72)]
        [InlineData(37, Doelgroep.Tussen55En60, 4000, 35, 271.96)]
        public void TestBerekenDoelgroepVermindering_Tussen55En60_BrutoloonKleinerDanMinBrutoloon
            (int werkuren, Doelgroep doelgroep, double brutoloon, double urenPerWeek, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            double doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, Doelgroep.Tussen55En60, 4700, 37, 0)]
        [InlineData(37, Doelgroep.Tussen55En60, 5400, 23, 0)]
        [InlineData(37, Doelgroep.Tussen55En60, 5000, 35, 0)]
        public void TestBerekenDoelgroepVermindering_Tussen55En60_BrutoloonGroterDanMinBrutoloon
            (int werkuren, Doelgroep doelgroep, double brutoloon, double urenPerWeek, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            double doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, Doelgroep.Tussen55En60, 4466.66, 37, 0)]
        [InlineData(37, Doelgroep.Tussen55En60, 4466.66, 23, 0)]
        [InlineData(37, Doelgroep.Tussen55En60, 4466.66, 35, 0)]
        public void TestBerekenDoelgroepVermindering_Tussen55En60_BrutoloonGelijkAanMinBrutoloon
            (int werkuren, Doelgroep doelgroep, double brutoloon, double urenPerWeek, double expected)

        {
            Werkgever.AantalWerkuren = werkuren;
            double doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion

        #region Vanaf60
        [Theory]
        [InlineData(37, Doelgroep.Vanaf60, 1700, 37, 375.00)]
        [InlineData(37, Doelgroep.Vanaf60, 3546, 23, 233.11)]
        [InlineData(37, Doelgroep.Vanaf60, 4000, 35, 354.73)]
        public void TestBerekenDoelgroepVermindering_Vanaf60_BrutoloonKleinerDanMinBrutoloon
            (int werkuren, Doelgroep doelgroep, double brutoloon, double urenPerWeek, double expected)
        {
            Werkgever.AantalWerkuren = 37;
            double doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(2850, 30);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(304.05, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, Doelgroep.Vanaf60, 5000, 37, 0)]
        [InlineData(37, Doelgroep.Vanaf60, 4865, 23, 0)]
        [InlineData(37, Doelgroep.Vanaf60, 4380, 35, 0)]
        public void TestBerekenDoelgroepVermindering_Vanaf60_BrutoloonGroterDanMinBrutoloon
            (int werkuren, Doelgroep doelgroep, double brutoloon, double urenPerWeek, double expected)
        {
            Werkgever.AantalWerkuren = 37;
            double doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(4700, 30);
            Assert.Equal(0, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, Doelgroep.Vanaf60, 4466.66, 37, 0)]
        [InlineData(37, Doelgroep.Vanaf60, 4466.66, 23, 0)]
        [InlineData(37, Doelgroep.Vanaf60, 4466.66, 35, 0)]
        public void TestBerekenDoelgroepVermindering_Vanaf60_BrutoloonGelijkAanMinBrutoloon
            (int werkuren, Doelgroep doelgroep, double brutoloon, double urenPerWeek, double expected)
        {
            Werkgever.AantalWerkuren = 37;
            double doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(4466.66, 30);
            Assert.Equal(0, doelgroepVermindering);
        }
        #endregion

        #region Andere
        [Theory]
        [InlineData(37, Doelgroep.Andere, 1865, 37, 0)]
        [InlineData(37, Doelgroep.Andere, 2500, 23, 0)]
        [InlineData(37, Doelgroep.Andere, 4466.66, 35, 0)]
        public void TestBerekenDoelgroepVermindering_Andere
            (int werkuren, Doelgroep doelgroep, double brutoloon, double urenPerWeek, double expected)
        // returned sowieso steeds 0
        {
            Werkgever.AantalWerkuren = werkuren;
            double doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion
    }
}
