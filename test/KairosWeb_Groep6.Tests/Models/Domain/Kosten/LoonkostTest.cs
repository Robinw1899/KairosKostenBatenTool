using System;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using Xunit;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Tests.Models.Domain.Kosten
{
    public class LoonkostTest
    {
        private Loonkost _loonkost;

        private int _aantalWerkuren;

        private double patronaleBijdrage = 35D;

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
                Doelgroep = Doelgroep.LaaggeschooldTot25,
                Ondersteuningspremie = 20,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0D
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

            Assert.Equal(0, _loonkost.BerekenBrutoloonPerMaand(_aantalWerkuren, patronaleBijdrage));
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

            Assert.Equal(0, _loonkost.BerekenBrutoloonPerMaand(_aantalWerkuren, patronaleBijdrage));
        }

        [Theory]
        [InlineData(37, 1800, 37, Doelgroep.LaaggeschooldTot25, 2430.00)]
        [InlineData(37, 2200, 23, Doelgroep.MiddengeschooldTot25, 1846.22)]
        [InlineData(37, 1900, 35, Doelgroep.Tussen55En60, 2426.35)]
        public void TestBerekenBrutoloonPerMaand_AlleGegevensIngevuld
            (int werkuren, double brutoloon, int urenPerWeek, Doelgroep doelgroep, double expected)
        {
            _aantalWerkuren = werkuren;

            _loonkost = new Loonkost
            {
                BrutoMaandloonFulltime = brutoloon,
                AantalUrenPerWeek = urenPerWeek,
                Doelgroep = doelgroep
            };

            double brutoloonPerMaand = _loonkost.BerekenBrutoloonPerMaand(_aantalWerkuren, patronaleBijdrage);
            // afronden omdat je werkt met doubles, de excel is ook afgerond op 2 decimalen
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

            Assert.Equal(0,_loonkost.BerekenGemiddeldeVOPPerMaand(_aantalWerkuren, patronaleBijdrage));
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

            Assert.Equal(0, _loonkost.BerekenGemiddeldeVOPPerMaand(_aantalWerkuren, patronaleBijdrage));
        }

        [Theory]
        [InlineData(1800, 37, Doelgroep.LaaggeschooldTot25, 20, 408.50)]
        [InlineData(2200, 23, Doelgroep.MiddengeschooldTot25, 30, 507.24)]
        [InlineData(3540, 35, Doelgroep.Tussen55En60, 40, 1699.49)]
        [InlineData(4300, 30, Doelgroep.Vanaf60, 0, 0)]
        public void TestBerekenGemiddeldeVOPPerMaand_AlleGegevensIngevuld
            (double brutoloon, double urenPerWeerk, Doelgroep doelgroep, double VOP, double expected)
        {
            _aantalWerkuren = 37;

            _loonkost = new Loonkost
            {
                BrutoMaandloonFulltime = brutoloon,
                AantalUrenPerWeek = urenPerWeerk,
                Doelgroep = doelgroep,
                Ondersteuningspremie = VOP
            };

            double gemiddeldeVopPerMaand = _loonkost.BerekenGemiddeldeVOPPerMaand(_aantalWerkuren, patronaleBijdrage);
            // afronden omdat je werkt met doubles, de excel is ook afgerond op 2 decimalen
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
                Doelgroep = Doelgroep.LaaggeschooldTot25,
                Ondersteuningspremie = 20
            };

            Assert.Equal(0, _loonkost.BerekenTotaleLoonkost(_aantalWerkuren, patronaleBijdrage));
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
                IBOPremie = 0D
            };

            Assert.Equal(0, _loonkost.BerekenGemiddeldeVOPPerMaand(_aantalWerkuren, patronaleBijdrage));
        }

        [Fact]
        public void TestBerekenTotaleLoonkost_AlleGegevensIngevuld()
        {
            _aantalWerkuren = 37;

            _loonkost = new Loonkost
            {
                BrutoMaandloonFulltime = 2000,
                AantalUrenPerWeek = 23,
                Doelgroep = Doelgroep.LaaggeschooldTot25,
                Ondersteuningspremie = 20,
                AantalMaandenIBO = 2,
                IBOPremie = 564.0D
            };

            double totaleLoonkost = _loonkost.BerekenTotaleLoonkost(_aantalWerkuren, patronaleBijdrage);
            // afronden omdat je werkt met doubles, de excel is ook afgerond op 2 decimalen
            totaleLoonkost = Math.Round(totaleLoonkost, 2);
            Assert.Equal(14272.00, totaleLoonkost);
        }
    }
}
