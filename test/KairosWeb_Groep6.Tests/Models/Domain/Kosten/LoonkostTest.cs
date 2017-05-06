using System;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Xunit;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Tests.Models.Domain.Kosten
{
    public class LoonkostTest
    {
        #region Properties
        private Loonkost _loonkost;

        private int _aantalWerkuren;

        private const decimal PatronaleBijdrage = 35M;

        private const string Laaggeschoold = "Wn's < 25 jaar laaggeschoold";
        private const string Middengeschoold = "Wn's < 25 jaar middengeschoold";
        private const string Tussen55En60 = "Wn's ≥ 55 en < 60 jaar";
        private const string Vanaf60 = "Wns ≥ 60 jaar";
        #endregion
        
        [Fact]
        public void TestConstructorSetsTypeEnSoort()
        {
            _loonkost = new Loonkost();
            Assert.Equal(Type.Kost, _loonkost.Type);
            Assert.Equal(Soort.Loonkost, _loonkost.Soort);
        }

        [Fact]
        public void TestSetBedrag_DoetNiets()
        {
            _aantalWerkuren = 37;

            _loonkost = new Loonkost
            {
                BrutoMaandloonFulltime = 2000,
                AantalUrenPerWeek = 23,
                Doelgroep = new Doelgroep(Laaggeschoold, 2500M, 1550M),
                Ondersteuningspremie = 20,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0M
            };

            Assert.Equal(0, _loonkost.Bedrag);
        }

        [Fact]
        public void TestBerekenBrutoloonPerMaand_GegevenOntbreekt_Returns0()
        {
            // Werkgever.AantalWerkuren ontbreekt (standaard 0)
            _aantalWerkuren = 0;
            _loonkost = new Loonkost
            {
                BrutoMaandloonFulltime = 2000,
                AantalUrenPerWeek = 23
            };

            Assert.Equal(0, _loonkost.BerekenBrutoloonPerMaand(_aantalWerkuren, PatronaleBijdrage));
        }

        [Fact]
        public void TestBerekenBrutoloonPerMaand_GegevenGelijkAan0_Returns0()
        {
            _aantalWerkuren = 37;

            _loonkost = new Loonkost
            {
                BrutoMaandloonFulltime = 2000,
                AantalUrenPerWeek = 0
            };

            Assert.Equal(0, _loonkost.BerekenBrutoloonPerMaand(_aantalWerkuren, PatronaleBijdrage));
        }

        [Theory]
        [InlineData(37, 1800, 37, Laaggeschoold, 2500, 1550, 2430.00)]
        [InlineData(37, 2200, 23, Middengeschoold, 2500, 1000, 1846.22)]
        [InlineData(37, 1900, 35, Tussen55En60, 4466.66, 1150, 2426.35)]
        public void TestBerekenBrutoloonPerMaand_AlleGegevensIngevuld
            (int werkuren, decimal brutoloon, int urenPerWeek, string omschrijving, 
            decimal minBrutoloon, decimal doelgroepvermindering, decimal expected)
        {
            _aantalWerkuren = werkuren;

            _loonkost = new Loonkost
            {
                BrutoMaandloonFulltime = brutoloon,
                AantalUrenPerWeek = urenPerWeek,
                Doelgroep = new Doelgroep(omschrijving, minBrutoloon, doelgroepvermindering)
            };

            decimal brutoloonPerMaand = _loonkost.BerekenBrutoloonPerMaand(_aantalWerkuren, PatronaleBijdrage);
            // afronden omdat je werkt met decimals, de excel is ook afgerond op 2 decimalen
            brutoloonPerMaand = Math.Round(brutoloonPerMaand, 2);
            Assert.Equal(expected, brutoloonPerMaand);
        }

        [Fact]
        public void TestBerekenGemiddeldeVOPPerMaand_GegevenOntbreekt_Returns0()
        {
            // doelgroep ontbreekt
            _aantalWerkuren = 37;

            _loonkost = new Loonkost
            {
                BrutoMaandloonFulltime = 2000,
                AantalUrenPerWeek = 23
            };

            Assert.Equal(0,_loonkost.BerekenGemiddeldeVOPPerMaand(_aantalWerkuren, PatronaleBijdrage));
        }

        [Fact]
        public void TestBerekenGemiddeldeVOPPerMaand_DoelgroepNull_Returns0()
        {
            _aantalWerkuren = 37;

            _loonkost = new Loonkost
            {
                BrutoMaandloonFulltime = 2000,
                AantalUrenPerWeek = 23,
                Doelgroep = null,
                Ondersteuningspremie = 30
            };

            Assert.Equal(0, _loonkost.BerekenGemiddeldeVOPPerMaand(_aantalWerkuren, PatronaleBijdrage));
        }

        [Theory]
        [InlineData(1800, 37, Laaggeschoold, 2500, 1550, 20, 408.50)]
        [InlineData(2200, 23, Middengeschoold, 2500, 1000, 30, 507.24)]
        [InlineData(3540, 35, Tussen55En60, 4466.66, 1150, 40, 1699.49)]
        [InlineData(4300, 30, Vanaf60, 4466.66, 1500,0, 0)]
        public void TestBerekenGemiddeldeVOPPerMaand_AlleGegevensIngevuld
            (decimal brutoloon, decimal urenPerWeerk, string omschrijving, decimal minBrutoloon,
            decimal doelgroepvermindering, decimal VOP, decimal expected)
        {
            _aantalWerkuren = 37;

            _loonkost = new Loonkost
            {
                BrutoMaandloonFulltime = brutoloon,
                AantalUrenPerWeek = urenPerWeerk,
                Doelgroep = new Doelgroep(omschrijving, minBrutoloon, doelgroepvermindering),
                Ondersteuningspremie = VOP
            };

            decimal gemiddeldeVopPerMaand = _loonkost.BerekenGemiddeldeVOPPerMaand(_aantalWerkuren, PatronaleBijdrage);
            // afronden omdat je werkt met decimals, de excel is ook afgerond op 2 decimalen
            gemiddeldeVopPerMaand = Math.Round(gemiddeldeVopPerMaand, 2);
            Assert.Equal(expected, gemiddeldeVopPerMaand);
        }

        [Fact]
        public void TestBerekenTotaleLoonkost_GegevenOntbreekt_Returns0()
        {
            // IBOPremie en AantalMaandenIBO ontbreken
            _aantalWerkuren = 37;

            _loonkost = new Loonkost
            {
                BrutoMaandloonFulltime = 2000,
                AantalUrenPerWeek = 23,
                Doelgroep = new Doelgroep(Laaggeschoold, 2500M, 1550M),
                Ondersteuningspremie = 20
            };

            Assert.Equal(0, _loonkost.BerekenTotaleLoonkost(_aantalWerkuren, PatronaleBijdrage));
        }

        [Fact]
        public void TestBerekenTotaleLoonkost_DoelgroepNull_Returns0()
        {
            _aantalWerkuren = 37;

            _loonkost = new Loonkost
            {
                BrutoMaandloonFulltime = 2000,
                AantalUrenPerWeek = 23,
                Doelgroep = null,
                Ondersteuningspremie = 0,
                AantalMaandenIBO = 0,
                IBOPremie = 0
            };

            Assert.Equal(0, _loonkost.BerekenGemiddeldeVOPPerMaand(_aantalWerkuren, PatronaleBijdrage));
        }

        [Fact]
        public void TestBerekenTotaleLoonkost_AlleGegevensIngevuld()
        {
            _aantalWerkuren = 37;

            _loonkost = new Loonkost
            {
                BrutoMaandloonFulltime = 2000,
                AantalUrenPerWeek = 23,
                Doelgroep = new Doelgroep(Laaggeschoold, 2500M, 1550M),
                Ondersteuningspremie = 20,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0M
            };

            decimal totaleLoonkost = _loonkost.BerekenTotaleLoonkost(_aantalWerkuren, PatronaleBijdrage);
            // afronden omdat je werkt met decimals, de excel is ook afgerond op 2 decimalen
            totaleLoonkost = Math.Round(totaleLoonkost, 2);
            Assert.Equal(14272.00M, totaleLoonkost);
        }
    }
}
