using System;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Kosten
{
    public class DoelgroepTest
    {
        private decimal patronaleBijdrage = 35M;
        private Doelgroep doelgroep;

        #region Gegevens ontbreken
        [Fact]
        public void TestBerekenDoelgroepVermindering_GegevensOntbreken()
        {
            // brutoloon, MinBrutoloon en StandaardDoelgroepVermindering ontbreken
            doelgroep = new Doelgroep {Soort = DoelgroepSoort.LaaggeschooldTot25};
            Assert.Throws<InvalidOperationException>(() => doelgroep.BerekenDoelgroepVermindering(0, 30, 23, 35));
        }
        #endregion

        #region LaaggeschooldTot25
        [Theory]
        [InlineData(37, DoelgroepSoort.LaaggeschooldTot25, 1800, 37, 387.5)]
        [InlineData(37, DoelgroepSoort.LaaggeschooldTot25, 2450, 23, 240.88)]
        [InlineData(37, DoelgroepSoort.LaaggeschooldTot25, 1500, 35, 366.55)]
        public void TestBerekenDoelgroepVermindering_LaaggeschooldTot25_BrutoloonKleinerDanMinBrutoloon
            (int werkuren, DoelgroepSoort soort, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            doelgroep = new Doelgroep(soort, 2500M, 1550M);

            decimal doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, DoelgroepSoort.LaaggeschooldTot25, 2650, 37, 0)]
        [InlineData(37, DoelgroepSoort.LaaggeschooldTot25, 3000, 23, 0)]
        [InlineData(37, DoelgroepSoort.LaaggeschooldTot25, 5000, 35, 0)]
        public void TestBerekenDoelgroepVermindering_LaaggeschooldTot25_BrutoloonGroterDanMinBrutoloon
            (int werkuren, DoelgroepSoort soort, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            doelgroep = new Doelgroep(soort, 2500M, 1550M);

            decimal doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, DoelgroepSoort.LaaggeschooldTot25, 2500, 37, 0)]
        [InlineData(37, DoelgroepSoort.LaaggeschooldTot25, 2500, 23, 0)]
        [InlineData(37, DoelgroepSoort.LaaggeschooldTot25, 2500, 35, 0)]
        public void TestBerekenDoelgroepVermindering_LaaggeschooldTot25_BrutoloonGelijkAanMinBrutoloon
            (int werkuren, DoelgroepSoort soort, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            doelgroep = new Doelgroep(soort, 2500M, 1550M);

            decimal doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion

        #region MiddengeschooldTot25
        [Theory]
        [InlineData(37, DoelgroepSoort.MiddengeschooldTot25, 1800, 37, 250.00)]
        [InlineData(37, DoelgroepSoort.MiddengeschooldTot25, 2200, 23, 155.41)]
        [InlineData(37, DoelgroepSoort.MiddengeschooldTot25, 1900, 35, 236.49)]
        public void TestBerekenDoelgroepVermindering_MiddengeschooldTot25_BrutoloonKleinerDanMinBrutoloon
            (int werkuren, DoelgroepSoort soort, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            doelgroep = new Doelgroep(soort, 2500M, 1000M);

            decimal doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, DoelgroepSoort.MiddengeschooldTot25, 2890, 37, 0)]
        [InlineData(37, DoelgroepSoort.MiddengeschooldTot25, 3100, 23, 0)]
        [InlineData(37, DoelgroepSoort.MiddengeschooldTot25, 2756, 35, 0)]
        public void TestBerekenDoelgroepVermindering_MiddengeschooldTot25_BrutoloonGroterDanMinBrutoloon
            (int werkuren, DoelgroepSoort soort, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            doelgroep = new Doelgroep(soort, 2500M, 1000M);

            decimal doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, DoelgroepSoort.MiddengeschooldTot25, 2500, 37, 0)]
        [InlineData(37, DoelgroepSoort.MiddengeschooldTot25, 2500, 23, 0)]
        [InlineData(37, DoelgroepSoort.MiddengeschooldTot25, 2500, 35, 0)]
        public void TestBerekenDoelgroepVermindering_MiddengeschooldTot25_BrutoloonGelijkAanMinBrutoloon
            (int werkuren, DoelgroepSoort soort, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            doelgroep = new Doelgroep(soort, 2500M, 1000M);

            decimal doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion

        #region Tussen55En60
        [Theory]
        [InlineData(37, DoelgroepSoort.Tussen55En60, 2378, 37, 287.50)]
        [InlineData(37, DoelgroepSoort.Tussen55En60, 3850, 23, 178.72)]
        [InlineData(37, DoelgroepSoort.Tussen55En60, 4000, 35, 271.96)]
        public void TestBerekenDoelgroepVermindering_Tussen55En60_BrutoloonKleinerDanMinBrutoloon
            (int werkuren, DoelgroepSoort soort, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            doelgroep = new Doelgroep(soort, 4466.66M, 1150M);

            decimal doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, DoelgroepSoort.Tussen55En60, 4700, 37, 0)]
        [InlineData(37, DoelgroepSoort.Tussen55En60, 5400, 23, 0)]
        [InlineData(37, DoelgroepSoort.Tussen55En60, 5000, 35, 0)]
        public void TestBerekenDoelgroepVermindering_Tussen55En60_BrutoloonGroterDanMinBrutoloon
            (int werkuren, DoelgroepSoort soort, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            doelgroep = new Doelgroep(soort, 4466.66M, 1150M);

            decimal doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, DoelgroepSoort.Tussen55En60, 4466.66, 37, 0)]
        [InlineData(37, DoelgroepSoort.Tussen55En60, 4466.66, 23, 0)]
        [InlineData(37, DoelgroepSoort.Tussen55En60, 4466.66, 35, 0)]
        public void TestBerekenDoelgroepVermindering_Tussen55En60_BrutoloonGelijkAanMinBrutoloon
            (int werkuren, DoelgroepSoort soort, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            doelgroep = new Doelgroep(soort, 4466.66M, 1150M);

            decimal doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion

        #region Vanaf60
        [Theory]
        [InlineData(37, DoelgroepSoort.Vanaf60, 1700, 37, 375.00)]
        [InlineData(37, DoelgroepSoort.Vanaf60, 3546, 23, 233.11)]
        [InlineData(37, DoelgroepSoort.Vanaf60, 4000, 35, 354.73)]
        public void TestBerekenDoelgroepVermindering_Vanaf60_BrutoloonKleinerDanMinBrutoloon
            (int werkuren, DoelgroepSoort soort, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            doelgroep = new Doelgroep(soort, 4466.66M, 1500M);

            decimal doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, DoelgroepSoort.Vanaf60, 5000, 37, 0)]
        [InlineData(37, DoelgroepSoort.Vanaf60, 4865, 23, 0)]
        [InlineData(37, DoelgroepSoort.Vanaf60, 4500, 35, 0)]
        public void TestBerekenDoelgroepVermindering_Vanaf60_BrutoloonGroterDanMinBrutoloon
            (int werkuren, DoelgroepSoort soort, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            doelgroep = new Doelgroep(soort, 4466.66M, 1500M);

            decimal doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, DoelgroepSoort.Vanaf60, 4466.66, 37, 0)]
        [InlineData(37, DoelgroepSoort.Vanaf60, 4466.66, 23, 0)]
        [InlineData(37, DoelgroepSoort.Vanaf60, 4466.66, 35, 0)]
        public void TestBerekenDoelgroepVermindering_Vanaf60_BrutoloonGelijkAanMinBrutoloon
            (int werkuren, DoelgroepSoort soort, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            doelgroep = new Doelgroep(soort, 4466.66M, 1500M);

            decimal doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion

        #region Andere
        [Theory]
        [InlineData(37, DoelgroepSoort.Andere, 1865, 37, 0, 1550, 1000)]
        [InlineData(37, DoelgroepSoort.Andere, 2500, 23, 0, 1000, 1000)]
        [InlineData(37, DoelgroepSoort.Andere, 4466.66, 35, 0, 2500, 1000)]
        public void TestBerekenDoelgroepVermindering_Andere
            (int werkuren, DoelgroepSoort soort, decimal brutoloon, decimal urenPerWeek, decimal expected, 
            decimal minBrutoloon, decimal standaardvermindering)
        // returned sowieso steeds 0
        {
            doelgroep = new Doelgroep(soort, minBrutoloon, standaardvermindering);

            decimal doelgroepVermindering = doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion

        #region ToString        
        [Theory]
        [InlineData(DoelgroepSoort.LaaggeschooldTot25, "Wn's < 25 jaar laaggeschoold")]
        [InlineData(DoelgroepSoort.MiddengeschooldTot25, "Wn's < 25 jaar middengeschoold")]
        [InlineData(DoelgroepSoort.Tussen55En60, "Wn's ≥ 55 en < 60 jaar")]
        [InlineData(DoelgroepSoort.Vanaf60, "Wns ≥ 60 jaar")]
        [InlineData(DoelgroepSoort.Andere, "Andere")]
        public void TestGeefOmschrijving(DoelgroepSoort soort, string expected)
        {
            doelgroep = new Doelgroep
            {
                Soort = soort
            };

            string omschrijving = doelgroep.ToString();

            Assert.Equal(expected, omschrijving);
        }
        #endregion
    }
}
