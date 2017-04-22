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
        private readonly ExtraOmzetController _controller;

        private readonly Mock<IAnalyseRepository> _analyseRepo;

        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public ExtraOmzetControllerTest()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();

            _analyseRepo = new Mock<IAnalyseRepository>();
            _controller = new ExtraOmzetController(_analyseRepo.Object);
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

            Assert.Equal(12000M, model?.JaarbedragOmzetverlies);
            Assert.Equal(5M, model?.Besparing);
        }
        #endregion

        #region Opslaan
        [Fact]
        public void TestOpslaan_RedirectsToIndex()
        {
            ExtraOmzetViewModel model = new ExtraOmzetViewModel
            {
                JaarbedragOmzetverlies = 25000,
                Besparing = 10
            };

            var result = _controller.Opslaan(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            Assert.Equal(model.JaarbedragOmzetverlies, _analyse.ExtraOmzet.JaarbedragOmzetverlies);
            Assert.Equal(model.Besparing, _analyse.ExtraOmzet.Besparing);
        }
        #endregion
    }
}
