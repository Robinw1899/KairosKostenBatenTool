﻿using System;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using Xunit;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Tests.Models.Domain.Baten
{
    public class MedewerkerNiveauBaatTest
    {
        private MedewerkerNiveauBaat _baat;

        #region Zelfde niveau
        [Fact]
        public void TestConstructorSetsTypeEnSoort_LagerNiveau()
        {
            _baat = new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau);
            Assert.Equal(Soort.MedewerkersZelfdeNiveau, _baat.Soort);
            Assert.Equal(Type.Baat, _baat.Type);
        }

        [Theory]
        [InlineData(0, 37, 3250, 0)]
        [InlineData(37, 0, 3250, 0)]
        [InlineData(37, 30, 0, 0)]
        public void TestBerekenTotaleLoonkostPerJaar_LagerNiveau_GegevenOntbreekt_Returns0
            (int werkuren, int uren, double brutoloon, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            _baat = new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
            {
                Uren = uren,
                BrutoMaandloonFulltime = brutoloon
            };

            double totaleLoonkostPerJaar = _baat.BerekenTotaleLoonkostPerJaar();
            Assert.Equal(expected, totaleLoonkostPerJaar);
        }

        [Theory]
        [InlineData(37, 35, 2300, 40885.30)]
        [InlineData(37, 30, 2000, 30473.51)]
        [InlineData(37, 37, 3250, 61074.00)]
        public void TestBerekenTotaleLoonkostPerJaar_LagerNiveau_AlleGegevensAanwezig
            (int werkuren, int uren, double brutoloon, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            _baat = new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
            {
                Uren = uren,
                BrutoMaandloonFulltime = brutoloon
            };

            double totaleLoonkostPerJaar = _baat.BerekenTotaleLoonkostPerJaar();
            totaleLoonkostPerJaar = Math.Round(totaleLoonkostPerJaar, 2);

            Assert.Equal(expected, totaleLoonkostPerJaar);
        }

        [Theory]
        [InlineData(37, 35, 2300, 40885.30)]
        [InlineData(37, 30, 2000, 30473.51)]
        [InlineData(37, 37, 3250, 61074.00)]
        public void TestBedragReturnsTotaleLoonkostPerJaar_LagerNiveau
            (int werkuren, int uren, double brutoloon, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            _baat = new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
            {
                Uren = uren,
                BrutoMaandloonFulltime = brutoloon
            };

            double totaleLoonkostPerJaar = Math.Round(_baat.Bedrag, 2);

            Assert.Equal(expected, totaleLoonkostPerJaar);
        }
        #endregion

        #region Hoger niveau
        [Fact]
        public void TestConstructorSetsTypeEnSoort_HogerNiveau()
        {
            _baat = new MedewerkerNiveauBaat(Soort.MedewerkersHogerNiveau);
            Assert.Equal(Soort.MedewerkersHogerNiveau, _baat.Soort);
            Assert.Equal(Type.Baat, _baat.Type);
        }

        [Theory]
        [InlineData(0, 37, 3250, 0)]
        [InlineData(37, 0, 3250, 0)]
        [InlineData(37, 30, 0, 0)]
        public void TestBerekenTotaleLoonkostPerJaar_HogerNiveau_GegevenOntbreekt_Returns0
            (int werkuren, int uren, double brutoloon, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            _baat = new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
            {
                Uren = uren,
                BrutoMaandloonFulltime = brutoloon
            };

            double totaleLoonkostPerJaar = _baat.BerekenTotaleLoonkostPerJaar();
            Assert.Equal(0, totaleLoonkostPerJaar);
        }

        [Theory]
        [InlineData(37, 28, 2300, 32708.24)]
        [InlineData(37, 32, 2860, 46482.27)]
        [InlineData(37, 37, 3250, 61074.00)]
        public void TestBerekenTotaleLoonkostPerJaar_HogerNiveau_AlleGegevensAanwezig
            (int werkuren, int uren, double brutoloon, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            _baat = new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
            {
                Uren = uren,
                BrutoMaandloonFulltime = brutoloon
            };

            double totaleLoonkostPerJaar = _baat.BerekenTotaleLoonkostPerJaar();
            totaleLoonkostPerJaar = Math.Round(totaleLoonkostPerJaar, 2);

            Assert.Equal(expected, totaleLoonkostPerJaar);
        }

        [Theory]
        [InlineData(37, 28, 2300, 32708.24)]
        [InlineData(37, 32, 2860, 46482.27)]
        [InlineData(37, 37, 3250, 61074.00)]
        public void TestBedragReturnsTotaleLoonkostPerJaar_HogerNiveau
            (int werkuren, int uren, double brutoloon, double expected)
        {
            Werkgever.AantalWerkuren = werkuren;
            _baat = new MedewerkerNiveauBaat(Soort.MedewerkersZelfdeNiveau)
            {
                Uren = uren,
                BrutoMaandloonFulltime = brutoloon
            };

            double totaleLoonkostPerJaar = Math.Round(_baat.Bedrag, 2);

            Assert.Equal(expected, totaleLoonkostPerJaar);
        }
        #endregion
    }
}