using System;
using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Baten;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Tests.Data;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Extensions
{
    public class KostOfBaatExtensionsTest
    {
        private readonly DummyApplicationDbContext _dbContext;

        public KostOfBaatExtensionsTest()
        {
            Werkgever.AantalWerkuren = 37;
            _dbContext = new DummyApplicationDbContext();
        }

        [Fact]
        public void TestGetBy_GeenGevonden_ReturnsNull()
        {
            List<KostOfBaat> loonkosten = new List<KostOfBaat>(_dbContext.Loonkosten);
            KostOfBaat kostOfBaat = loonkosten.GetBy(0);
            Assert.Null(kostOfBaat);
        }

        #region Loonkosten
        [Fact]
        public void TestGetBy_Loonkosten()
        {
            List<KostOfBaat> loonkosten = new List<KostOfBaat>(_dbContext.Loonkosten);
            KostOfBaat kostOfBaat = loonkosten.GetBy(2);
            // omzetten naar de juiste klasse:
            Loonkost loonkost = (Loonkost) kostOfBaat;
            Assert.Equal(_dbContext.Secretaresse, loonkost);
        }

        [Fact]
        public void TestGeefTotaal_Loonkosten()
        {
            List<KostOfBaat> loonkosten = new List<KostOfBaat>(_dbContext.Loonkosten);
            // totaal van alle loonkosten:
            double totaal = loonkosten.GeefTotaal();
            totaal = Math.Round(totaal, 2);

            Assert.Equal(57837.13, totaal);
        }
        #endregion

        #region ExtraKosten 
        [Fact]
        public void TestGetBy_ExtaKosten()
        {
            List<KostOfBaat>  extraKosten = new List<KostOfBaat>(_dbContext.ExtraKosten);
            KostOfBaat kostOfBaat = extraKosten.GetBy(3);
            // omzetten naar de juiste klasse:
            ExtraKost extrakost = (ExtraKost)kostOfBaat;

            Assert.Equal(400, extrakost.Bedrag);
            Assert.Equal("Boeken en ander studiemateriaal", extrakost.Beschrijving);
        }

        [Fact]
        public void TestGeefTotaal_ExtaKosten()
        {
            List<KostOfBaat>  extraKosten = new List<KostOfBaat>(_dbContext.ExtraKosten);
            // totaal van alle loonkosten:
            double totaal = extraKosten.GeefTotaal();
            totaal = Math.Round(totaal, 2);
 
            Assert.Equal(1550.00, totaal);
        }
        #endregion

        #region MedewerkerNiveauBaat
        [Fact]
        public void TestGetBy_MedewerkerNiveauBaat()
        {
            List<KostOfBaat> medewerkerNiveauBaten = new List<KostOfBaat>(_dbContext.MedewerkerNiveauBaten);
            KostOfBaat kostOfBaat = medewerkerNiveauBaten.GetBy(1);
            // omzetten naar de juiste klasse:
            MedewerkerNiveauBaat baat = (MedewerkerNiveauBaat)kostOfBaat;

            Assert.Equal(2300, baat.BrutoMaandloonFulltime);
            Assert.Equal(35, baat.Uren);
        }

        [Fact]
        public void TestBedragReturnsBerekening_MedewerkerNiveauBaat()
        {
            List<KostOfBaat>  medewerkerNiveauBaten = new List<KostOfBaat>(_dbContext.MedewerkerNiveauBaten);
            KostOfBaat kostOfBaat = medewerkerNiveauBaten.GetBy(1);
            // omzetten naar de juiste klasse:
            MedewerkerNiveauBaat baat = (MedewerkerNiveauBaat)kostOfBaat;
            double bedrag = Math.Round(baat.Bedrag, 2);
            Assert.Equal(40885.30, bedrag);
        }

        [Fact]
        public void TestGeefTotaal_MedewerkerNiveauBaat()
        {
            List<KostOfBaat>  medewerkerNiveauBaten = new List<KostOfBaat>(_dbContext.MedewerkerNiveauBaten);
            // totaal van alle loonkosten:
            double totaal = medewerkerNiveauBaten.GeefTotaal();
            totaal = Math.Round(totaal, 2);

            Assert.Equal(132432.81, totaal);
        }
        #endregion

        #region Subsidie
        [Fact]
        public void TestGetBy_Subsidie()
        {
            List<KostOfBaat> subsidies = new List<KostOfBaat>(_dbContext.Subsidies);
            KostOfBaat kostOfBaat = subsidies.GetBy(2);
            // omzetten naar de juiste klasse:
            Subsidie subsidie = (Subsidie)kostOfBaat;

            Assert.Equal(1500, subsidie.Bedrag);
        }

        [Fact]
        public void TestGeefTotaal_Subsidie()
        {
            List<KostOfBaat> subsidies = new List<KostOfBaat>(_dbContext.Subsidies);
            // totaal van alle loonkosten:
            double totaal = subsidies.GeefTotaal();
            totaal = Math.Round(totaal, 2);

            Assert.Equal(1700.00, totaal);
        }
        #endregion
    }
}
