using System;
using System.Collections.Generic;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Extensions;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Tests.Data;
using Xunit;

namespace KairosWeb_Groep6.Tests.Models.Domain.Extensions
{
    public class KostOfBaatExtensionsTest
    {
        private readonly List<KostOfBaat> _kostenOfBaten;
        private readonly DummyApplicationDbContext _dbContext;

        public KostOfBaatExtensionsTest()
        {
            Werkgever.AantalWerkuren = 37;
            _dbContext = new DummyApplicationDbContext();
            _kostenOfBaten = new List<KostOfBaat>(_dbContext.Loonkosten);
        }

        [Fact]
        public void TestGetBy_GeenGevonden_ReturnsNull()
        {
            KostOfBaat kostOfBaat = _kostenOfBaten.GetBy(0);
            Assert.Null(kostOfBaat);
        }

        [Fact]
        public void TestGetBy()
        {
            KostOfBaat kostOfBaat = _kostenOfBaten.GetBy(2);
            // omzetten naar de juiste klasse:
            Loonkost loonkost = (Loonkost) kostOfBaat;
            Assert.Equal(loonkost, _dbContext.Secretaresse);
        }

        [Fact]
        public void TestGeefTotaal()
        {
            // totaal van alle loonkosten:
            double totaal = _kostenOfBaten.GeefTotaal();
            totaal = Math.Round(totaal, 2);

            Assert.Equal(57837.13, totaal);
        }
    }
}
