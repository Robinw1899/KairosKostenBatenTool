using System;
using KairosWeb_Groep6.Controllers.Baten;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers.Baten
{
    public class LogistiekeBesparingControllerTest
    {
        #region Properties
        private readonly LogistiekeBesparingController _controller;
        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public LogistiekeBesparingControllerTest()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();

            var analyseRepo = new Mock<IAnalyseRepository>();
            _controller = new LogistiekeBesparingController(analyseRepo.Object);
            _analyse = new Analyse
            {
                LogistiekeBesparing = dbContext.LogistiekeBesparing
            };

            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index
        [Fact]
        public void TestIndex_ReturnsView()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            var model = result?.Model as LogistiekeBesparingViewModel;

            var transportkosten = Convert.ToDecimal(model?.TransportKosten);
            var handlingskosten = Convert.ToDecimal(model?.LogistiekHandlingsKosten);

            Assert.Equal(3000M, transportkosten);
            Assert.Equal(2000M, handlingskosten);
        }

        [Fact]
        public void TestIndex_AnalyseKlaar_RedirectsToResultaat()
        {
            Analyse analyse = new Analyse
            {
                Klaar = true
            };

            var result = _controller.Index(analyse) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Resultaat", result?.ControllerName);
        }
        #endregion

        #region Opslaan
        [Fact]
        public void TestOpslaan_RedirectsToIndex()
        {
            LogistiekeBesparingViewModel model = new LogistiekeBesparingViewModel
            {
                TransportKosten = "" + 25000,
                LogistiekHandlingsKosten = "" + 2830
            };

            var result = _controller.Opslaan(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            var expectedTransportkosten = Convert.ToDecimal(model.TransportKosten);
            var actualTransportkosten = Convert.ToDecimal(_analyse.LogistiekeBesparing.TransportKosten);
            var expectedHandlingskosten = Convert.ToDecimal(model.LogistiekHandlingsKosten);
            var actualHandlingskosten = Convert.ToDecimal(_analyse.LogistiekeBesparing.LogistiekHandlingsKosten);

            Assert.Equal(expectedTransportkosten, actualTransportkosten);
            Assert.Equal(expectedHandlingskosten, actualHandlingskosten);
        }
        #endregion
    }
}
