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
        private readonly ExtraOmzetController _controller;
        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public ExtraOmzetControllerTest()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();

            var analyseRepo = new Mock<IAnalyseRepository>();
            _controller = new ExtraOmzetController(analyseRepo.Object);
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
        #endregion

        #region Opslaan
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
