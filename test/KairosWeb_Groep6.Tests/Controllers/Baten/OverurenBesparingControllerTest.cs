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
    public class OverurenBesparingControllerTest
    {
        #region Properties
        private readonly Mock<IAnalyseRepository> _analyseRepository;
        private readonly Mock<IExceptionLogRepository> _exceptionLogRepository;
        private readonly OverurenBesparingController _controller;
        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public OverurenBesparingControllerTest()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();
            _analyseRepository = new Mock<IAnalyseRepository>();
            _exceptionLogRepository = new Mock<IExceptionLogRepository>();

            _controller = new OverurenBesparingController(_analyseRepository.Object, _exceptionLogRepository.Object);
            _analyse = new Analyse
            {
                OverurenBesparing = dbContext.OverurenBesparing
            };

            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index
        [Fact]
        public void TestIndex_ReturnsView()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            var model = result?.Model as OverurenBesparingViewModel;

            var bedrag = Convert.ToDecimal(model?.Bedrag);

            Assert.Equal(34570M, bedrag);
        }

        [Fact]
        public void TestIndex_AnalyseKlaar_RedirectsToResultaat()
        {
            Analyse analyse = new Analyse
            {
                InArchief = true
            };

            var result = _controller.Index(analyse) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Resultaat", result?.ControllerName);
        }
        #endregion

        #region Opslaan
        [Fact]
        public void TestOpslaan_RepoGooitException_RedirectsToIndex()
        {
            _analyseRepository.Setup(r => r.Save()).Throws(new Exception());
            OverurenBesparingViewModel model = new OverurenBesparingViewModel();

            var result = _controller.Opslaan(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestOpslaan_RedirectsToIndex()
        {
            OverurenBesparingViewModel model = new OverurenBesparingViewModel
            {
                Bedrag = "" + 25000
            };

            var result = _controller.Opslaan(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            var expected = Convert.ToDecimal(model.Bedrag);
            var actual = Convert.ToDecimal(_analyse.OverurenBesparing.Bedrag);

            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
