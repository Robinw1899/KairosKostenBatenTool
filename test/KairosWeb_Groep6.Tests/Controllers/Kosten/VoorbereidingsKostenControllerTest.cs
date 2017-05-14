using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Controllers.Kosten;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Tests.Controllers.Kosten
{
    public class VoorbereidingsKostenControllerTest
    {
        #region Properties
        private readonly Mock<IAnalyseRepository> _analyseRepo;
        private readonly Mock<IExceptionLogRepository> _exceptionLogRepository;
        private readonly VoorbereidingsKostenController _controller;
        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public VoorbereidingsKostenControllerTest()
        {
            var dbContext = new DummyApplicationDbContext();
            _analyseRepo = new Mock<IAnalyseRepository>();
            _exceptionLogRepository = new Mock<IExceptionLogRepository>();

            _controller = new VoorbereidingsKostenController(_analyseRepo.Object, _exceptionLogRepository.Object);
            _analyse = new Analyse { VoorbereidingsKosten = dbContext.VoorbereidingsKosten };

            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index -- GET --
        [Fact]
        public void TestIndex_ShouldReturnUitzendKrachtBesparingViewModels()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            IEnumerable<VoorbereidingsKostViewModel> model =
                result?.Model as IEnumerable<VoorbereidingsKostViewModel>;

            Assert.Equal(3, model?.Count());
        }

        [Fact]
        public void TestIndex_AnalyseKlaar_RedirectsToResultaat()
        {
            Analyse analyse = new Analyse
            {
                InArchief = true
            };

            var result = _controller.Index(analyse) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Resultaat", result?.ControllerName);
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
        public void TestVoegToe_RepoGooitException_RedirectsToIndex()
        {
            _analyseRepo.Setup(r => r.Save()).Throws(new Exception());
            VoorbereidingsKostViewModel model = new VoorbereidingsKostViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save());
        }

        [Fact]
        public void TestVoegToe_ModelError_RedirectsToIndex()
        {
            _controller.ModelState.AddModelError("", "Model error");
            VoorbereidingsKostViewModel model = new VoorbereidingsKostViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVoegToe_Succes_RedirectsToIndex()
        {
            VoorbereidingsKostViewModel model = new VoorbereidingsKostViewModel
            {
                Id = 1,
                Type = Type.Kost,
                Soort = Soort.VoorbereidingsKost,
                Beschrijving = "test",
                Bedrag = "" + 9208
            };
            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal(4, _analyse.VoorbereidingsKosten.Count);
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

            VoorbereidingsKostViewModel model = new VoorbereidingsKostViewModel();
            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestBewerk_RepoGooiException_RedirectsToIndex()
        {
            _analyseRepo.Setup(r => r.Save()).Throws(new Exception());
            VoorbereidingsKostViewModel model = new VoorbereidingsKostViewModel{Id = 1};
            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save());
        }

        [Fact]
        public void TestBewerk_KostNull_RedirectsToIndex()
        {
            VoorbereidingsKostViewModel model = new VoorbereidingsKostViewModel
            {
                Id = -1,
                Type = Type.Kost,
                Soort = Soort.VoorbereidingsKost,
                Beschrijving = "test",
                Bedrag = "" + 9208
            };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestBewerk_Succes_RedirectsToIndex()
        {
            VoorbereidingsKostViewModel model = new VoorbereidingsKostViewModel
            {
                Id = 1,
                Type = Type.Kost,
                Soort = Soort.VoorbereidingsKost,
                Beschrijving = "test",
                Bedrag = "" + 9208
            };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        #endregion

        #region Verwijder -- POST --
        [Fact]
        public void TestVerwijder_RepoGooiException_RedirectsToIndex()
        {
            _analyseRepo.Setup(r => r.Save()).Throws(new Exception());

            var result = _controller.Verwijder(_analyse, 1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save());
        }

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
