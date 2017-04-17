using KairosWeb_Groep6.Controllers.Kosten;
using KairosWeb_Groep6.Data.Repositories;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten.GereedschapsKostenViewModels;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers.Kosten
{
    class GereedschapsKostControllerTest
    {
        private readonly GereedschapsKostenController _controller;
        private readonly Analyse _analyse;
        private readonly Mock<AnalyseRepository> _analyseRepo;
        private readonly DummyApplicationDbContext _dbContext;

        public GereedschapsKostControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _analyseRepo = new Mock<AnalyseRepository>();
            _controller = new GereedschapsKostenController(_analyseRepo.Object);
            _analyse = new Analyse { GereedschapsKosten = _dbContext.GeefGereedschapsKosten() };
            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #region Index -- GET
        [Fact]
        public void TestIndexShouldReturnInfrastructuurKostenIndexViewModel()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            GereedschapsKostenIndexViewModel model = result?.Model as GereedschapsKostenIndexViewModel;

            Assert.Equal(3, model?.ViewModels.Count());
        }
        #endregion
        #region VoegToe -- Post
        [Fact]
        public void TestVoegToe_ModelError_ReturnsToIndexWithModel()
        {
            _controller.ModelState.AddModelError("", "Model error");
            GereedschapsKostenIndexViewModel model = new GereedschapsKostenIndexViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVoegToe_Succes_ReturnsPartialView()
        {
            GereedschapsKostenIndexViewModel model = new GereedschapsKostenIndexViewModel()
            {
                Id = 1,
                Type = KairosWeb_Groep6.Models.Domain.Type.Kost,
                Soort = Soort.InfrastructuurKost,
                Beschrijving = "",
                Bedrag = 3500,
                ViewModels = _dbContext.GeefGereedschapsKosten()
                                        .Select(b => new GereedschapsKostenViewModel(b))
            };

            var result = _controller.VoegToe(_analyse, model) as PartialViewResult;

            Assert.Equal("_OverzichtTabel", result?.ViewName);
        }
        #endregion

        #region Bewerk -- Get
        [Fact]
        public void TestBewerk_ReturnsIndexView()
        {
            var result = _controller.Bewerk(_analyse, 1) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }
        [Fact]
        public void TestBewerkGet_GereedschapsKostNull_ReturnsIndexView()
        {
            var result = _controller.Bewerk(_analyse, -1) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }
        #endregion
        #region Bewerk -- Post
        [Fact]
        public void TestBewerk_ModelError_ReturnsIndexView()
        {
            _controller.ModelState.AddModelError("", "Model error");
            GereedschapsKostenIndexViewModel model = new GereedschapsKostenIndexViewModel();
            var result = _controller.Bewerk(_analyse, model) as ViewResult;
            Assert.Equal("Index", result?.ViewName);
        }
        [Fact]
        public void TestBewerkPost_KostNull_ReturnsIndexView()
        {
            GereedschapsKostenIndexViewModel model = new GereedschapsKostenIndexViewModel
            {
                Id = -1,
                Type = KairosWeb_Groep6.Models.Domain.Type.Kost,
                Soort = Soort.GereedschapsKost,
                Beschrijving = "",
                Bedrag = 2000,
                ViewModels = _dbContext.GeefGereedschapsKosten()
                                       .Select(b => new GereedschapsKostenViewModel(b))
            };
            var result = _controller.Bewerk(_analyse, model) as ViewResult;
            var modelResult = result?.Model;

            Assert.Equal("Index", result?.ViewName);
            Assert.Equal(model, modelResult);
        }
        [Fact]
        public void TestBewerk_Succes_ReturnsToIndex()
        {
            GereedschapsKostenIndexViewModel model = new GereedschapsKostenIndexViewModel
            {
                Id = 1,
                Type = KairosWeb_Groep6.Models.Domain.Type.Kost,
                Soort = Soort.GereedschapsKost,
                Beschrijving = "",
                Bedrag = 2000,
                ViewModels = _dbContext.GeefGereedschapsKosten()
                                       .Select(b => new GereedschapsKostenViewModel(b))
            };
            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }
        #endregion

        [Fact]
        public void TestVerwijderGetByGeeftNull_MethodeFaaltNiet()
        {
            var result = _controller.Verwijder(_analyse, -1) as ViewResult;
            Assert.Equal("Index", result?.ViewName);
        }
        [Fact]
        public void TestVerwijder_Succes_RedirectsToIndexView()
        {
            var result = _controller.Verwijder(_analyse, 1) as ViewResult;
            Assert.Equal("Index", result?.ViewName);
        }
    }
}
