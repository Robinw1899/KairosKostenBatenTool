using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Controllers.Baten;
using KairosWeb_Groep6.Data.Repositories;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers.Baten
{
    class ExterneInkopenControllerTest
    {
        #region Properties
        private readonly ExterneInkopenController _controller;
        private readonly Analyse _analyse;
        private readonly Mock<AnalyseRepository> _analyseRepo;
        private readonly DummyApplicationDbContext _dbContext;
        #endregion

        #region Constructors
        public ExterneInkopenControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _analyseRepo = new Mock<AnalyseRepository>();

            _controller = new ExterneInkopenController(_analyseRepo.Object);
            _analyse = new Analyse { ExterneInkopen = _dbContext.ExterneInkopen };

            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index -- GET --
        [Fact]
        public void TestIndex_ShouldReturnUitzendKrachtBesparingViewModels()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            IEnumerable<ExterneInkoopViewModel> model =
                result?.Model as IEnumerable<ExterneInkoopViewModel>;

            Assert.Equal(3, model?.Count());
        }
        #endregion

        #region VoegToe -- GET --
        [Fact]
        public void TestVoegToe()
        {
            var result = _controller.VoegToe() as PartialViewResult;

            Assert.Equal("_Formulier", result?.ViewName);
        }
        #endregion

        #region VoegToe -- POST --
        [Fact]
        public void TestVoegToe_ModelError_RedirectsToIndex()
        {
            _controller.ModelState.AddModelError("", "Model error");
            ExterneInkoopViewModel model = new ExterneInkoopViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVoegToe_Succes_RedirectsToIndex()
        {
            ExterneInkoopViewModel model = new ExterneInkoopViewModel()
            {
                Id = 1,
                Type = KairosWeb_Groep6.Models.Domain.Type.Kost,
                Soort = Soort.ExterneInkoop,
                Beschrijving = "inkoop papierwerk",
                Bedrag = 2750
            };
            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal(4, _analyse.ExterneInkopen.Count);
        }
        #endregion

        #region Bewerk -- GET --
        [Fact]
        public void TestBewerk_ReturnsPartialView()
        {
            var result = _controller.Bewerk(_analyse, 1) as PartialViewResult;

            Assert.Equal("_Formulier", result?.ViewName);
        }

        [Fact]
        public void TestBewerkGET_BaatNull_RedirectsToIndex()
        {
            var result = _controller.Bewerk(_analyse, -1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }
        #endregion

        #region Bewerk -- POST --
        [Fact]
        public void TestBewerk_ModelError_RedirectsToIndex()
        {
            _controller.ModelState.AddModelError("", "Model error");

            ExterneInkoopViewModel model = new ExterneInkoopViewModel();
            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestBewerk_BaatNull_RedirectsToIndex()
        {
            ExterneInkoopViewModel model = new ExterneInkoopViewModel()
            {
                Id = -1,
                Type = Type.Kost,
                Soort = Soort.ExterneInkoop,
                Beschrijving = "test",
                Bedrag = 2900
            };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestBewerk_Succes_ReturnsPartialView()
        {
            ExterneInkoopViewModel model = new ExterneInkoopViewModel()
            {
                Id = 1,
                Type = Type.Kost,
                Soort = Soort.MedewerkersZelfdeNiveau,
                Beschrijving = "test",
                Bedrag = 2750
            };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("_Formulier", result?.ActionName);
        }

        #endregion

        #region Verwijder -- POST --
        [Fact]
        public void TestVerwijder_BaatNull_MethodeFaaltNiet()
        {
            var result = _controller.Verwijder(_analyse, -1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVerwijder_Succes_RedirectsToIndex()
        {
            var result = _controller.Verwijder(_analyse, 1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }
        #endregion
    }
}
