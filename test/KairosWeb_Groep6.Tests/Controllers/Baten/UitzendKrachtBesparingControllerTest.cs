using System.Linq;
using KairosWeb_Groep6.Controllers.Baten;
using KairosWeb_Groep6.Data.Repositories;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Baten.MedewerkerNiveauBaatViewModels;
using KairosWeb_Groep6.Models.KairosViewModels.Baten.UitzendKrachtBesparingViewModels;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace KairosWeb_Groep6.Tests.Controllers.Baten
{
    public class UitzendKrachtBesparingControllerTest
    {
        private readonly UitzendKrachtBesparingenController _controller;
        private readonly Analyse _analyse;
        private readonly Mock<AnalyseRepository> _analyseRepo;
        private readonly DummyApplicationDbContext _dbContext;

        public UitzendKrachtBesparingControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _analyseRepo = new Mock<AnalyseRepository>();
            _controller = new UitzendKrachtBesparingenController(_analyseRepo.Object);
            _analyse = new Analyse { UitzendKrachtBesparingen = _dbContext.GeefUitzendKrachtBesparingen() };
            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }

        #region Index -- GET --
        [Fact]
        public void TestIndexShouldReturnUitzendKrachtBesparingIndexViewModel()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            UitzendKrachtBesparingIndexViewModel model = result?.Model as UitzendKrachtBesparingIndexViewModel;

            Assert.Equal(3, model?.ViewModels.Count());
        }

        #endregion

        #region VoegToe -- POST --

        [Fact]
        public void TestVoegToe_ModelError_ReturnsToIndexWithModel()
        {
            _controller.ModelState.AddModelError("", "Model error");
            UitzendKrachtBesparingIndexViewModel model = new UitzendKrachtBesparingIndexViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVoegToe_Succes_ReturnsPartialView()
        {
            UitzendKrachtBesparingIndexViewModel model = new UitzendKrachtBesparingIndexViewModel()
            {
                Id = 1,
                Type = Type.Baat,
                Soort = Soort.MedewerkersZelfdeNiveau,
                Beschrijving = "Tuinier",
                Bedrag = 2500,
                ViewModels = _dbContext.GeefUitzendKrachtBesparingen()
                                        .Select(b => new UitzendKrachtBesparingViewModel(b))
            };

            var result = _controller.VoegToe(_analyse, model) as PartialViewResult;

            Assert.Equal("_OverzichtTabel", result?.ViewName);
        }

        #endregion

        #region Bewerk -- GET --

        [Fact]
        public void TestBewerk_ReturnsIndexView()
        {
            var result = _controller.Bewerk(_analyse, 1) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void TestBewerkGET_BaatNull_ReturnsIndexView()
        {
            var result = _controller.Bewerk(_analyse, -1) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }
        #endregion

        #region Bewerk -- POST --
        [Fact]
        public void TestBewerk_ModelError_ReturnsIndexView()
        {
            _controller.ModelState.AddModelError("", "Model error");

            UitzendKrachtBesparingIndexViewModel model = new UitzendKrachtBesparingIndexViewModel();
            var result = _controller.Bewerk(_analyse, model) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void TestBewerkPOST_BaatNull_ReturnsIndexView()
        {
            UitzendKrachtBesparingIndexViewModel model = new UitzendKrachtBesparingIndexViewModel()
            {
                Id = -1,
                Type = Type.Baat,
                Soort = Soort.MedewerkersZelfdeNiveau,
                Beschrijving = "Tuinier",
                Bedrag = 2500,
                ViewModels = _dbContext.GeefUitzendKrachtBesparingen()
                                        .Select(b => new UitzendKrachtBesparingViewModel(b))
            };

            var result = _controller.Bewerk(_analyse, model) as ViewResult;

            var modelResult = result?.Model;

            Assert.Equal("Index", result?.ViewName);
            Assert.Equal(model, modelResult);
        }

        [Fact]
        public void TestBewerk_Succes_RedirectsToIndex()
        {
            UitzendKrachtBesparingIndexViewModel model = new UitzendKrachtBesparingIndexViewModel()
            {
                Id = 1,
                Type = Type.Baat,
                Soort = Soort.MedewerkersZelfdeNiveau,
                Beschrijving = "Tuinier",
                Bedrag = 2500,
                ViewModels = _dbContext.GeefUitzendKrachtBesparingen()
                                        .Select(b => new UitzendKrachtBesparingViewModel(b))
            };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        #endregion

        #region Verwijder -- POST --

        [Fact]
        public void TestVerwijder_GetByGeeftNull_MethodeFaaltNiet()
        {
            var result = _controller.Verwijder(_analyse, -1) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void TestVerwijder_Succes_RedirectsToIndex()
        {
            var result = _controller.Verwijder(_analyse, 1) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }

        #endregion
    }
}
