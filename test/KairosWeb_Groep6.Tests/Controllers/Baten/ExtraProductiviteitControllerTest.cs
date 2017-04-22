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
    public class ExtraProductiviteitControllerTest
    {
        #region Properties
        private readonly ExtraProductiviteitController _controller;

        private readonly Mock<IAnalyseRepository> _analyseRepo;

        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public ExtraProductiviteitControllerTest()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();

            _analyseRepo = new Mock<IAnalyseRepository>();
            _controller = new ExtraProductiviteitController(_analyseRepo.Object);
            _analyse = new Analyse
            {
                ExtraProductiviteit = dbContext.ExtraProductiviteit
            };

            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index
        [Fact]
        public void TestIndex_ReturnsView()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            var model = result?.Model as ExtraProductiviteitViewModel;

            Assert.Equal(6470M, model?.Bedrag);
        }
        #endregion

        #region Opslaan
        [Fact]
        public void TestOpslaan_RedirectsToIndex()
        {
            ExtraProductiviteitViewModel model = new ExtraProductiviteitViewModel
            {
                Bedrag = 25000
            };

            var result = _controller.Opslaan(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            Assert.Equal(model.Bedrag, _analyse.ExtraProductiviteit.Bedrag);
        }
        #endregion
    }
}
