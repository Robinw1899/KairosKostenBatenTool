﻿using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Controllers.Kosten;
using KairosWeb_Groep6.Data.Repositories;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.Domain.Kosten;
using KairosWeb_Groep6.Models.KairosViewModels.Kosten;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Tests.Controllers.Kosten
{
    public class LoonkostenControllerTest
    {
        #region Properties
        private readonly Mock<IAnalyseRepository> _analyseRepository;
        private readonly Mock<IExceptionLogRepository> _exceptionLogRepository;
        private readonly DummyApplicationDbContext _dbContext;
        private readonly LoonkostenController _controller;
        private readonly Analyse _analyse;
        private readonly Mock<IDoelgroepRepository> _doelgroepRepository;

        private const string Laaggeschoold = "Wn's < 25 jaar laaggeschoold";
        #endregion

        #region Constructors
        public LoonkostenControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _analyseRepository = new Mock<IAnalyseRepository>();
            _exceptionLogRepository = new Mock<IExceptionLogRepository>();
            _doelgroepRepository = new Mock<IDoelgroepRepository>();

            _controller = new LoonkostenController(_analyseRepository.Object, _doelgroepRepository.Object, _exceptionLogRepository.Object);
            _analyse = new Analyse { Loonkosten = _dbContext.Loonkosten };

            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index -- GET --
        [Fact]
        public void TestIndex_ShouldReturnUitzendKrachtBesparingViewModels()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            IEnumerable<LoonkostViewModel> model =
                result?.Model as IEnumerable<LoonkostViewModel>;

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
            LoonkostFormViewModel model = new LoonkostFormViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVoegToe_RepositoryGooitException_ReturnsView()
        {
            _analyseRepository.Setup(r => r.Save()).Throws(new Exception());
            LoonkostFormViewModel model = new LoonkostFormViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestVoegToe_Succes_RedirectsToIndex()
        {
            _doelgroepRepository.Setup(r => r.GetById(1))
                .Returns(_dbContext.Laaggeschoold);

            LoonkostFormViewModel model = new LoonkostFormViewModel
            {
                Id = 4,
                Type = Type.Kost,
                Soort = Soort.Loonkost,
                BrutoMaandloonFulltime = "" + 1800,
                AantalUrenPerWeek = 37,
                doelgroep = 1,
                Ondersteuningspremie = 20,
                AantalMaandenIBO = 2,
                IBOPremie = "" + 564.0
            };

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal(4, _analyse.Loonkosten.Count);
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

            LoonkostViewModel model = new LoonkostViewModel();
            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestBewerk_RepoGooitException_RedirectsToIndex()
        {
            _analyseRepository.Setup(r => r.Save()).Throws(new Exception());
            LoonkostViewModel model = new LoonkostViewModel{Id = 1};

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestBewerk_KostNull_RedirectsToIndex()
        {
            LoonkostViewModel model = new LoonkostViewModel
            {
                Id = -1,
                Type = Type.Kost,
                Soort = Soort.Loonkost,
                BrutoMaandloonFulltime = "" + 1800,
                AantalUrenPerWeek = 37,
                Doelgroep = new Doelgroep(Laaggeschoold, 2500M, 1550M),
                Ondersteuningspremie = 20,
                AantalMaandenIBO = 2,
                IBOPremie = "" + 564.0M
            };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestBewerk_Succes_RedirectsToIndex()
        {
            LoonkostViewModel model = new LoonkostViewModel
            {
                Id = 1,
                Type = Type.Kost,
                Soort = Soort.Loonkost,
                BrutoMaandloonFulltime = "" + 1800,
                AantalUrenPerWeek = 37,
                Doelgroep = new Doelgroep(Laaggeschoold, 2500M, 1550M),
                Ondersteuningspremie = 20,
                AantalMaandenIBO = 2,
                IBOPremie = "" + 564.0M
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
