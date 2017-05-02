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
    public class GereedschapsKostenControllerTest
    {
        #region Properties
        private readonly GereedschapsKostenController _controller;
        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public GereedschapsKostenControllerTest()
        {
            var dbContext = new DummyApplicationDbContext();
            var analyseRepo = new Mock<AnalyseRepository>();

            _controller = new GereedschapsKostenController(analyseRepo.Object);
            _analyse = new Analyse { GereedschapsKosten = dbContext.GereedschapsKosten };

            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index -- GET --
        [Fact]
        public void TestIndex_ShouldReturnUitzendKrachtBesparingViewModels()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            IEnumerable<GereedschapsKostViewModel> model =
                result?.Model as IEnumerable<GereedschapsKostViewModel>;

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
            GereedschapsKostViewModel model = new GereedschapsKostViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVoegToe_Succes_RedirectsToIndex()
        {
            GereedschapsKostViewModel model = new GereedschapsKostViewModel()
            {
                Id = 1,
                Type = Type.Kost,
                Soort = Soort.GereedschapsKost,
                Beschrijving = "test",
                Bedrag = "" + 9208
            };
            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal(4, _analyse.GereedschapsKosten.Count);
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
        public void TestBewerkGET_KostNull_RedirectsToIndex()
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

            GereedschapsKostViewModel model = new GereedschapsKostViewModel();
            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestBewerk_KostNull_RedirectsToIndex()
        {
            GereedschapsKostViewModel model = new GereedschapsKostViewModel()
            {
                Id = -1,
                Type = Type.Kost,
                Soort = Soort.GereedschapsKost,
                Beschrijving = "test",
                Bedrag = "" + 9208
            };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestBewerk_Succes_RedirectsToIndex()
        {
            GereedschapsKostViewModel model = new GereedschapsKostViewModel()
            {
                Id = 1,
                Type = Type.Kost,
                Soort = Soort.GereedschapsKost,
                Beschrijving = "test",
                Bedrag = "" + 9208
            };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        #endregion

        #region Verwijder -- POST --
        [Fact]
        public void TestVerwijder_KostNull_MethodeFaaltNiet()
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
