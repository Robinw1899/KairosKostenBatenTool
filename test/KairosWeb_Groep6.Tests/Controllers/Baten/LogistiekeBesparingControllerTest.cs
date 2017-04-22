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

        private readonly Mock<IAnalyseRepository> _analyseRepo;

        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public LogistiekeBesparingControllerTest()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();

            _analyseRepo = new Mock<IAnalyseRepository>();
            _controller = new LogistiekeBesparingController(_analyseRepo.Object);
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

            Assert.Equal(3000M, model?.TransportKosten);
            Assert.Equal(2000M, model?.LogistiekHandlingsKosten);
        }
        #endregion

        #region Opslaan
        [Fact]
        public void TestOpslaan_RedirectsToIndex()
        {
            LogistiekeBesparingViewModel model = new LogistiekeBesparingViewModel
            {
                TransportKosten = 25000,
                LogistiekHandlingsKosten = 2830
            };

            var result = _controller.Opslaan(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            Assert.Equal(model.TransportKosten, _analyse.LogistiekeBesparing.TransportKosten);
            Assert.Equal(model.LogistiekHandlingsKosten, _analyse.LogistiekeBesparing.LogistiekHandlingsKosten);
        }
        #endregion
    }
}
