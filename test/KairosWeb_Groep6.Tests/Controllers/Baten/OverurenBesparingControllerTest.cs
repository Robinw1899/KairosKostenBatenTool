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
        private readonly OverurenBesparingController _controller;

        private readonly Mock<IAnalyseRepository> _analyseRepo;

        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public OverurenBesparingControllerTest()
        {
            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();

            _analyseRepo = new Mock<IAnalyseRepository>();
            _controller = new OverurenBesparingController(_analyseRepo.Object);
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

            Assert.Equal(34570M, model?.Bedrag);
        }
        #endregion

        #region Opslaan
        [Fact]
        public void TestOpslaan_RedirectsToIndex()
        {
            OverurenBesparingViewModel model = new OverurenBesparingViewModel
            {
                Bedrag = 25000
            };

            var result = _controller.Opslaan(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            Assert.Equal(model.Bedrag, _analyse.OverurenBesparing.Bedrag);
        }
        #endregion
    }
}
