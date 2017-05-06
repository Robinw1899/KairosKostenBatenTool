using System;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Kosten
{
    public class DoelgroepTest
    {
        #region Properties
        private decimal patronaleBijdrage = 35M;
        private Doelgroep _doelgroep;
        private const string Laaggeschoold = "Wn's < 25 jaar laaggeschoold";
        private const string Middengeschoold = "Wn's < 25 jaar middengeschoold";
        private const string Tussen55En60 = "Wn's ≥ 55 en < 60 jaar";
        private const string Vanaf60 = "Wns ≥ 60 jaar";
        private const string Andere = "Andere";
        #endregion

        #region Gegevens ontbreken
        [Fact]
        public void TestBerekenDoelgroepVermindering_GegevensOntbreken()
        {
            // brutoloon, MinBrutoloon en StandaardDoelgroepVermindering ontbreken
            _doelgroep = new Doelgroep {Omschrijving = Laaggeschoold};
            Assert.Equal(0, _doelgroep.BerekenDoelgroepVermindering(0, 30, 23, 35));
        }
        #endregion

        #region LaaggeschooldTot25
        [Theory]
        [InlineData(37, 1800, 37, 387.5)]
        [InlineData(37, 2450, 23, 240.88)]
        [InlineData(37, 1500, 35, 366.55)]
        public void TestBerekenDoelgroepVermindering_LaaggeschooldTot25_BrutoloonKleinerDanMinBrutoloon
            (int werkuren, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            _doelgroep = new Doelgroep(Laaggeschoold, 2500M, 1550M);

            decimal doelgroepVermindering = _doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, 2650, 37, 0)]
        [InlineData(37, 3000, 23, 0)]
        [InlineData(37, 5000, 35, 0)]
        public void TestBerekenDoelgroepVermindering_LaaggeschooldTot25_BrutoloonGroterDanMinBrutoloon
            (int werkuren, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            _doelgroep = new Doelgroep(Laaggeschoold, 2500M, 1550M);

            decimal doelgroepVermindering = _doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, 2500, 37, 0)]
        [InlineData(37, 2500, 23, 0)]
        [InlineData(37, 2500, 35, 0)]
        public void TestBerekenDoelgroepVermindering_LaaggeschooldTot25_BrutoloonGelijkAanMinBrutoloon
            (int werkuren, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            _doelgroep = new Doelgroep(Laaggeschoold, 2500M, 1550M);

            decimal doelgroepVermindering = _doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion

        #region MiddengeschooldTot25
        [Theory]
        [InlineData(37, 1800, 37, 250.00)]
        [InlineData(37, 2200, 23, 155.41)]
        [InlineData(37, 1900, 35, 236.49)]
        public void TestBerekenDoelgroepVermindering_MiddengeschooldTot25_BrutoloonKleinerDanMinBrutoloon
            (int werkuren, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            _doelgroep = new Doelgroep(Middengeschoold, 2500M, 1000M);

            decimal doelgroepVermindering = _doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, 2890, 37, 0)]
        [InlineData(37, 3100, 23, 0)]
        [InlineData(37, 2756, 35, 0)]
        public void TestBerekenDoelgroepVermindering_MiddengeschooldTot25_BrutoloonGroterDanMinBrutoloon
            (int werkuren, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            _doelgroep = new Doelgroep(Middengeschoold, 2500M, 1000M);

            decimal doelgroepVermindering = _doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, 2500, 37, 0)]
        [InlineData(37, 2500, 23, 0)]
        [InlineData(37, 2500, 35, 0)]
        public void TestBerekenDoelgroepVermindering_MiddengeschooldTot25_BrutoloonGelijkAanMinBrutoloon
            (int werkuren, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            _doelgroep = new Doelgroep(Middengeschoold, 2500M, 1000M);

            decimal doelgroepVermindering = _doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion

        #region Tussen55En60
        [Theory]
        [InlineData(37, 2378, 37, 287.50)]
        [InlineData(37, 3850, 23, 178.72)]
        [InlineData(37, 4000, 35, 271.96)]
        public void TestBerekenDoelgroepVermindering_Tussen55En60_BrutoloonKleinerDanMinBrutoloon
            (int werkuren, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            _doelgroep = new Doelgroep(Tussen55En60, 4466.66M, 1150M);

            decimal doelgroepVermindering = _doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, 4700, 37, 0)]
        [InlineData(37, 5400, 23, 0)]
        [InlineData(37, 5000, 35, 0)]
        public void TestBerekenDoelgroepVermindering_Tussen55En60_BrutoloonGroterDanMinBrutoloon
            (int werkuren, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            _doelgroep = new Doelgroep(Tussen55En60, 4466.66M, 1150M);

            decimal doelgroepVermindering = _doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, 4466.66, 37, 0)]
        [InlineData(37, 4466.66, 23, 0)]
        [InlineData(37, 4466.66, 35, 0)]
        public void TestBerekenDoelgroepVermindering_Tussen55En60_BrutoloonGelijkAanMinBrutoloon
            (int werkuren, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            _doelgroep = new Doelgroep(Tussen55En60, 4466.66M, 1150M);

            decimal doelgroepVermindering = _doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion

        #region Vanaf60
        [Theory]
        [InlineData(37, 1700, 37, 375.00)]
        [InlineData(37, 3546, 23, 233.11)]
        [InlineData(37, 4000, 35, 354.73)]
        public void TestBerekenDoelgroepVermindering_Vanaf60_BrutoloonKleinerDanMinBrutoloon
            (int werkuren, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            _doelgroep = new Doelgroep(Vanaf60, 4466.66M, 1500M);

            decimal doelgroepVermindering = _doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, 5000, 37, 0)]
        [InlineData(37, 4865, 23, 0)]
        [InlineData(37, 4500, 35, 0)]
        public void TestBerekenDoelgroepVermindering_Vanaf60_BrutoloonGroterDanMinBrutoloon
            (int werkuren, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            _doelgroep = new Doelgroep(Vanaf60, 4466.66M, 1500M);

            decimal doelgroepVermindering = _doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }

        [Theory]
        [InlineData(37, 4466.66, 37, 0)]
        [InlineData(37, 4466.66, 23, 0)]
        [InlineData(37, 4466.66, 35, 0)]
        public void TestBerekenDoelgroepVermindering_Vanaf60_BrutoloonGelijkAanMinBrutoloon
            (int werkuren, decimal brutoloon, decimal urenPerWeek, decimal expected)
        {
            _doelgroep = new Doelgroep(Vanaf60, 4466.66M, 1500M);

            decimal doelgroepVermindering = _doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion

        #region Andere
        [Theory]
        [InlineData(37, 1865, 37, 0, 1550, 1000)]
        [InlineData(37, 2500, 23, 0, 1000, 1000)]
        [InlineData(37, 4466.66, 35, 0, 2500, 1000)]
        public void TestBerekenDoelgroepVermindering_Andere
            (int werkuren, decimal brutoloon, decimal urenPerWeek, decimal expected, 
            decimal minBrutoloon, decimal standaardvermindering)
        // returned sowieso steeds 0
        {
            _doelgroep = new Doelgroep(Andere, minBrutoloon, standaardvermindering);

            decimal doelgroepVermindering = _doelgroep.BerekenDoelgroepVermindering(brutoloon, urenPerWeek, werkuren, patronaleBijdrage);
            doelgroepVermindering = Math.Round(doelgroepVermindering, 2);
            Assert.Equal(expected, doelgroepVermindering);
        }
        #endregion

        #region ToString        
        [Theory]
        [InlineData(Laaggeschoold)]
        [InlineData(Middengeschoold)]
        [InlineData(Tussen55En60)]
        [InlineData(Vanaf60)]
        [InlineData(Andere)]
        public void TestGeefOmschrijving(string expected)
        {
            _doelgroep = new Doelgroep
            {
                Omschrijving = expected
            };

            string actual = _doelgroep.ToString();

            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
