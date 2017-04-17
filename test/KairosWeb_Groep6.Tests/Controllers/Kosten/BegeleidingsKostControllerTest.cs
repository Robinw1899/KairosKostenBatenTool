using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Controllers.Kosten;
using KairosWeb_Groep6.Data.Repositories;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers.Kosten
{
    class BegeleidingsKostControllerTest
    {
        private readonly BegeleidingsKostenController _controller;
        private readonly Analyse _analyse;
        private readonly Mock<AnalyseRepository> _analyseRepo;
        private readonly DummyApplicationDbContext _dbContext;

        public BegeleidingsKostControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _analyseRepo = new Mock<AnalyseRepository>();
            _controller = new BegeleidingsKostenController(_analyseRepo.Object);
            _analyse = new Analyse { BegeleidingsKosten = _dbContext.BegeleidingsKosten };
            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #region Index -- GET
        [Fact]
        public void TestIndexShouldReturnInfrastructuurKostenIndexViewModel()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            IEnumerable<BegeleidingsKostViewModel> model = result?.Model as IEnumerable<BegeleidingsKostViewModel>;

            Assert.Equal(3, model?.Count());
        }
        #endregion

        #region VoegToe -- Post
        [Fact]
        public void TestVoegToe_ModelError_ReturnsToIndexWithModel()
        {
            _controller.ModelState.AddModelError("", "Model error");
            BegeleidingsKostenIndexViewModel model = new BegeleidingsKostenIndexViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVoegToe_Succes_ReturnsPartialView()
        {
            BegeleidingsKostenIndexViewModel model = new BegeleidingsKostenIndexViewModel()
            {
                Id = 1,
                Type = KairosWeb_Groep6.Models.Domain.Type.Kost,
                Soort = Soort.BegeleidingsKost,
                Uren = 8,
                BrutoMaandloonBegeleider = 2500,
                Bedrag = 30000,
                ViewModels = _dbContext.GeefBegeleidingsKosten()
                                        .Select(b => new BegeleidingsKostViewModel(b))
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
            BegeleidingsKostenIndexViewModel model = new BegeleidingsKostenIndexViewModel();
            var result = _controller.Bewerk(_analyse, model) as ViewResult;
            Assert.Equal("Index", result?.ViewName);
        }
        [Fact]
        public void TestBewerkPost_KostNull_ReturnsIndexView()
        {
            BegeleidingsKostenIndexViewModel model = new BegeleidingsKostenIndexViewModel
            {
                Id = -1,
                Type = KairosWeb_Groep6.Models.Domain.Type.Kost,
                Soort = Soort.BegeleidingsKost,
                Uren = 6,
                BrutoMaandloonBegeleider = 3000,
                Bedrag = 36000,
                ViewModels = _dbContext.GeefBegeleidingsKosten()
                                       .Select(b => new BegeleidingsKostViewModel(b))
            };
            var result = _controller.Bewerk(_analyse, model) as ViewResult;
            var modelResult = result?.Model;

            Assert.Equal("Index", result?.ViewName);
            Assert.Equal(model, modelResult);
        }
        [Fact]
        public void TestBewerk_Succes_ReturnsToIndex()
        {
            BegeleidingsKostenIndexViewModel model = new BegeleidingsKostenIndexViewModel
            {
                Id = 1,
                Type = KairosWeb_Groep6.Models.Domain.Type.Kost,
                Soort = Soort.BegeleidingsKost,
                Uren = 6,
                BrutoMaandloonBegeleider = 36000,
                Bedrag = 36000,
                ViewModels = _dbContext.GeefBegeleidingsKosten()
                                       .Select(b => new BegeleidingsKostViewModel(b))
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
