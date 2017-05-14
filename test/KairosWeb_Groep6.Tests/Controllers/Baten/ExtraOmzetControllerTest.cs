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
    public class ExtraOmzetControllerTest
    {
        #region Properties
        private readonly Mock<IAnalyseRepository> _analyseRepository;
        private readonly Mock<IExceptionLogRepository> _exceptionLogRepository;
        private readonly ExtraOmzetController _controller;
        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public ExtraOmzetControllerTest()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();
            _analyseRepository = new Mock<IAnalyseRepository>();
            _exceptionLogRepository = new Mock<IExceptionLogRepository>();

            _controller = new ExtraOmzetController(_analyseRepository.Object, _exceptionLogRepository.Object);
            _analyse = new Analyse
            {
                ExtraOmzet = dbContext.ExtraOmzet
            };

            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index
        [Fact]
        public void TestIndex_ReturnsView()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            var model = result?.Model as ExtraOmzetViewModel;
            var jaarbedrag = Convert.ToDecimal(model?.JaarbedragOmzetverlies);
            var besparing = Convert.ToDecimal(model?.Besparing);

            Assert.Equal(12000M, jaarbedrag);
            Assert.Equal(5M, besparing);
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
            ExtraOmzetViewModel model = new ExtraOmzetViewModel();

            var result = _controller.Opslaan(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestOpslaan_RedirectsToIndex()
        {
            ExtraOmzetViewModel model = new ExtraOmzetViewModel
            {
                JaarbedragOmzetverlies = "" + 25000,
                Besparing = "" + 10
            };

            var result = _controller.Opslaan(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            var expectedJaarbedrag = Convert.ToDecimal(model.JaarbedragOmzetverlies);
            var actualJaarbedrag = Convert.ToDecimal(_analyse.ExtraOmzet.JaarbedragOmzetverlies);
            var expectedBesparing = Convert.ToDecimal(model.Besparing);
            var actualBesparing = Convert.ToDecimal(_analyse.ExtraOmzet.Besparing);

            Assert.Equal(expectedJaarbedrag, actualJaarbedrag);
            Assert.Equal(expectedBesparing, actualBesparing);
        }
        #endregion
    }
}
