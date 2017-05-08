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
    public class SubsidieControllerTest
    {
        #region Properties
        private readonly SubsidieController _controller;
        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public SubsidieControllerTest()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();

            var analyseRepo = new Mock<IAnalyseRepository>();
            _controller = new SubsidieController(analyseRepo.Object);
            _analyse = new Analyse
            {
                Subsidie = dbContext.Subsidie
            };

            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index
        [Fact]
        public void TestIndex_ReturnsView()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            var model = result?.Model as SubsidieViewModel;

            var bedrag = Convert.ToDecimal(model?.Bedrag);

            Assert.Equal(3500M, bedrag);
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
            SubsidieViewModel model = new SubsidieViewModel
            {
                Bedrag = "" + 25000
            };

            var result = _controller.Opslaan(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            var expected = Convert.ToDecimal(model.Bedrag);
            var actual = Convert.ToDecimal(_analyse.Subsidie.Bedrag);

            Assert.Equal(expected, actual);
        }
        #endregion
    }
}
