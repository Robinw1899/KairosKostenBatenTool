using KairosWeb_Groep6.Controllers.Baten;
using KairosWeb_Groep6.Data.Repositories;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Baten.ExterneInkoopViewModels;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers.Baten
{
    class ExterneInkopenControllerTest
    {
        private readonly ExterneInkopenController _controller;
        private readonly Analyse _analyse;
        private readonly Mock<AnalyseRepository> _analyseRepo;
        private readonly DummyApplicationDbContext _dbContext;

        public ExterneInkopenControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _analyseRepo = new Mock<AnalyseRepository>();
            _controller = new ExterneInkopenController(_analyseRepo.Object);
            _analyse = new Analyse { ExterneInkopen = _dbContext.GeefExterneInkopen() };
            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }

        #region Index -- GET --
        [Fact]
        public void TestIndexShouldReturnMedewerkerNiveauIndexViewModel()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            ExterneInkopenIndexViewModel model = result?.Model as ExterneInkopenIndexViewModel;

            Assert.Equal(3, model?.ViewModels.Count());
        }

        #endregion

        #region VoegToe -- POST --

        [Fact]
        public void TestVoegToe_ModelError_ReturnsToIndexWithModel()
        {
            _controller.ModelState.AddModelError("", "Model error");
            ExterneInkopenIndexViewModel model = new ExterneInkopenIndexViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVoegToe_Succes_ReturnsPartialView()
        {
            ExterneInkopenIndexViewModel model = new ExterneInkopenIndexViewModel()
            {
                Id = 1,
                Type = KairosWeb_Groep6.Models.Domain.Type.Kost,
                Soort = Soort.ExterneInkoop,
                Beschrijving = "inkoop papierwerk",
                Bedrag = 2750,
                ViewModels = _dbContext.GeefExterneInkopen()
                                        .Select(b => new ExterneInkoopViewModel(b))
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

            ExterneInkopenIndexViewModel model = new ExterneInkopenIndexViewModel();
            var result = _controller.Bewerk(_analyse, model) as ViewResult;

            Assert.Equal("Index", result?.ViewName);
        }

        [Fact]
        public void TestBewerkPOST_BaatNull_ReturnsIndexView()
        {
            ExterneInkopenIndexViewModel model = new ExterneInkopenIndexViewModel()
            {
                Id = -1,
                Type = KairosWeb_Groep6.Models.Domain.Type.Kost,
                Soort = Soort.ExterneInkoop,
                Beschrijving = "test",
                Bedrag = 2900,
                ViewModels = _dbContext.GeefExterneInkopen()
                                        .Select(b => new ExterneInkoopViewModel(b))
            };

            var result = _controller.Bewerk(_analyse, model) as ViewResult;

            var modelResult = result?.Model;

            Assert.Equal("Index", result?.ViewName);
            Assert.Equal(model, modelResult);
        }

        [Fact]
        public void TestBewerk_Succes_RedirectsToIndex()
        {
            ExterneInkopenIndexViewModel model = new ExterneInkopenIndexViewModel()
            {
                Id = 1,
                Type = KairosWeb_Groep6.Models.Domain.Type.Kost,
                Soort = Soort.MedewerkersZelfdeNiveau,
                Beschrijving = "test",
                Bedrag = 2750,
                ViewModels = _dbContext.GeefExterneInkopen()
                                        .Select(b => new ExterneInkoopViewModel(b))
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
