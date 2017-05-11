using System;
using System.Linq;
using KairosWeb_Groep6.Controllers.Baten;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Tests.Controllers.Baten
{
    public class MedewerkerZelfdeNiveauControllerTest
    {
        #region Properties
        private readonly Mock<IAnalyseRepository> _analyseRepository;
        private readonly Mock<IExceptionLogRepository> _exceptionLogRepository;
        private readonly MedewerkersZelfdeNiveauController _controller;
        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public MedewerkerZelfdeNiveauControllerTest()
        {
            var dbContext = new DummyApplicationDbContext();
            _analyseRepository = new Mock<IAnalyseRepository>();
            _exceptionLogRepository = new Mock<IExceptionLogRepository>();

            _controller = new MedewerkersZelfdeNiveauController(_analyseRepository.Object, _exceptionLogRepository.Object);
            _analyse = new Analyse {MedewerkersZelfdeNiveauBaten = dbContext.MedewerkerNiveauBaten};

            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index -- GET --
        [Fact]
        public void TestIndex_ShouldReturnMedewerkerNiveauBaatViewModels()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            IEnumerable<MedewerkerNiveauBaatViewModel> model =
                result?.Model as IEnumerable<MedewerkerNiveauBaatViewModel>;

            Assert.Equal(6, model?.Count());
        }

        [Fact]
        public void TestIndex_AnalyseKlaar_RedirectsToResultaat()
        {
            Analyse analyse = new Analyse
            {
                Klaar = true
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
        public void TestVoegToe_ModelError_RedirectsToIndex()
        {
            _controller.ModelState.AddModelError("", "Model error");
            MedewerkerNiveauBaatViewModel model = new MedewerkerNiveauBaatViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVoegToe_RepositoryGooitException_ReturnsView()
        {
            _analyseRepository.Setup(r => r.Save()).Throws(new Exception());
            MedewerkerNiveauBaatViewModel model = new MedewerkerNiveauBaatViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestVoegToe_Succes_RedirectsToIndex()
        {
            MedewerkerNiveauBaatViewModel model = new MedewerkerNiveauBaatViewModel()
            {
                Id = 1,
                Type = Type.Baat,
                Soort = Soort.MedewerkersZelfdeNiveau,
                Uren = 37,
                BrutoMaandloonFulltime = "" + 3240
            };

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal(7, _analyse.MedewerkersZelfdeNiveauBaten.Count);
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

            MedewerkerNiveauBaatViewModel model = new MedewerkerNiveauBaatViewModel();
            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestBewerk_RepoGooitException_RedirectsToIndex()
        {
            _analyseRepository.Setup(r => r.Save()).Throws(new Exception());
            MedewerkerNiveauBaatViewModel model = new MedewerkerNiveauBaatViewModel { Id = 1 };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestBewerk_BaatNull_RedirectsToIndex()
        {
            MedewerkerNiveauBaatViewModel model = new MedewerkerNiveauBaatViewModel()
            {
                Id = -1,
                Type = Type.Baat,
                Soort = Soort.MedewerkersZelfdeNiveau,
                Uren = 37,
                BrutoMaandloonFulltime = "" + 3240
            };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestBewerk_Succes_RedirectsToIndex()
        {
            MedewerkerNiveauBaatViewModel model = new MedewerkerNiveauBaatViewModel()
            {
                Id = 1,
                Type = Type.Baat,
                Soort = Soort.MedewerkersZelfdeNiveau,
                Uren = 37,
                BrutoMaandloonFulltime = "" + 3240
            };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        #endregion

        #region Verwijder -- GET --
        [Fact]
        public void TestVerwijder_BaatNull_MethodeFaaltNiet()
        {
            var result = _controller.Verwijder(_analyse, -1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVerwijder_RepoGooitException_RedirectsToIndex()
        {
            _analyseRepository.Setup(r => r.Save()).Throws(new Exception());

            var result = _controller.Verwijder(_analyse, 1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
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
