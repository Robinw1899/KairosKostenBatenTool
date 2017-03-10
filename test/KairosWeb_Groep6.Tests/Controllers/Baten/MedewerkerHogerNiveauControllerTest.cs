using System.Linq;
using KairosWeb_Groep6.Controllers.Baten;
using KairosWeb_Groep6.Data.Repositories;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Baten.MedewerkerNiveauBaatViewModels;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers.Baten
{
    public class MedewerkerHogerNiveauControllerTest
    {
        private readonly MedewerkerHogerNiveauController _controller;
        private readonly Analyse _analyse;
        private readonly Mock<AnalyseRepository> _analyseRepo;
        private readonly DummyApplicationDbContext _dbContext;

        public MedewerkerHogerNiveauControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _analyseRepo = new Mock<AnalyseRepository>();
            _controller = new MedewerkerHogerNiveauController(_analyseRepo.Object);
            _analyse = new Analyse {MedewerkersHogerNiveauBaat = _dbContext.GeefMedewerkerNiveauBaten()};

        }

        #region Index -- GET --
        [Fact]
        public void TestIndexShouldReturnMedewerkerNiveauIndexViewModel()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            MedewerkerNiveauIndexViewModel model = result?.Model as MedewerkerNiveauIndexViewModel;

            Assert.Equal(3, model?.ViewModels.Count());
        }

        #endregion

        #region VoegToe -- POST --

        [Fact]
        public void TestVoegToe_ModelError_ReturnsToIndexWithModel()
        {
            _controller.ModelState.AddModelError("", "Model error");
            MedewerkerNiveauIndexViewModel model = new MedewerkerNiveauIndexViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVoegToe_Succes_ReturnsPartialView()
        {
            MedewerkerNiveauIndexViewModel model = new MedewerkerNiveauIndexViewModel()
            {
                Id = 1,
                Type = Type.Baat,
                Soort = Soort.MedewerkersZelfdeNiveau,
                Uren = 23,
                BrutoMaandloonFulltime = 2750,
                ViewModels = _dbContext.GeefMedewerkerNiveauBaten()
                                        .Select(b => new MedewerkerNiveauBaatViewModel(b))
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

            MedewerkerNiveauIndexViewModel model = new MedewerkerNiveauIndexViewModel();
            var result = _controller.Bewerk(_analyse, model) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void TestBewerkPOST_BaatNull_ReturnsIndexView()
        {
            MedewerkerNiveauIndexViewModel model = new MedewerkerNiveauIndexViewModel()
            {
                Id = -1,
                Type = Type.Baat,
                Soort = Soort.MedewerkersZelfdeNiveau,
                Uren = 23,
                BrutoMaandloonFulltime = 2750,
                ViewModels = _dbContext.GeefMedewerkerNiveauBaten()
                                        .Select(b => new MedewerkerNiveauBaatViewModel(b))
            };

            var result = _controller.Bewerk(_analyse, model) as ViewResult;

            var modelResult = result?.Model;

            Assert.Equal("Index", result?.ViewName);
            Assert.Equal(model, modelResult);
        }

        [Fact]
        public void TestBewerk_Succes_RedirectsToIndex()
        {
            MedewerkerNiveauIndexViewModel model = new MedewerkerNiveauIndexViewModel()
            {
                Id = 1,
                Type = Type.Baat,
                Soort = Soort.MedewerkersZelfdeNiveau,
                Uren = 23,
                BrutoMaandloonFulltime = 2750,
                ViewModels = _dbContext.GeefMedewerkerNiveauBaten()
                                        .Select(b => new MedewerkerNiveauBaatViewModel(b))
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
