﻿using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Controllers.Baten;
using KairosWeb_Groep6.Data.Repositories;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels.Baten;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Type = KairosWeb_Groep6.Models.Domain.Type;

namespace KairosWeb_Groep6.Tests.Controllers.Baten
{
    public class UitzendKrachtBesparingControllerTest
    {
        #region Properties
        private readonly Mock<IAnalyseRepository> _analyseRepository;
        private readonly Mock<IExceptionLogRepository> _exceptionLogRepository;
        private readonly UitzendKrachtBesparingenController _controller;
        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public UitzendKrachtBesparingControllerTest()
        {
            var dbContext = new DummyApplicationDbContext();
            _analyseRepository = new Mock<IAnalyseRepository>();
            _exceptionLogRepository = new Mock<IExceptionLogRepository>();

            _controller = new UitzendKrachtBesparingenController(_analyseRepository.Object, _exceptionLogRepository.Object);
            _analyse = new Analyse { UitzendKrachtBesparingen = dbContext.UitzendKrachtBesparingen };

            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index -- GET --
        [Fact]
        public void TestIndex_ShouldReturnUitzendKrachtBesparingViewModels()
        {
            var result = _controller.Index(_analyse) as ViewResult;
            IEnumerable<UitzendKrachtBesparingViewModel> model = 
                result?.Model as IEnumerable<UitzendKrachtBesparingViewModel>;

            Assert.Equal(5, model?.Count());
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
            UitzendKrachtBesparingViewModel model = new UitzendKrachtBesparingViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestVoegToe_RepositoryGooitException_ReturnsView()
        {
            _analyseRepository.Setup(r => r.Save()).Throws(new Exception());
            UitzendKrachtBesparingViewModel model = new UitzendKrachtBesparingViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestVoegToe_Succes_RedirectsToIndex()
        {
            UitzendKrachtBesparingViewModel model = new UitzendKrachtBesparingViewModel()
            {
                Id = 1,
                Type = Type.Baat,
                Soort = Soort.UitzendkrachtBesparing,
                Beschrijving = "Tuinier",
                Bedrag = "" + 2500
            };

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal(6, _analyse.UitzendKrachtBesparingen.Count);
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

            UitzendKrachtBesparingViewModel model = new UitzendKrachtBesparingViewModel();
            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestBewerk_RepoGooitException_RedirectsToIndex()
        {
            _analyseRepository.Setup(r => r.Save()).Throws(new Exception());
            UitzendKrachtBesparingViewModel model = new UitzendKrachtBesparingViewModel { Id = 1 };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestBewerk_BaatNull_RedirectsToIndex()
        {
            UitzendKrachtBesparingViewModel model = new UitzendKrachtBesparingViewModel()
            {
                Id = -1,
                Type = Type.Baat,
                Soort = Soort.UitzendkrachtBesparing,
                Beschrijving = "Tuinier",
                Bedrag = "" + 2500
            };

            var result = _controller.Bewerk(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestBewerk_Succes_RedirectsToIndex()
        {
            UitzendKrachtBesparingViewModel model = new UitzendKrachtBesparingViewModel()
            {
                Id = 1,
                Type = Type.Baat,
                Soort = Soort.UitzendkrachtBesparing,
                Beschrijving = "Tuinier",
                Bedrag = "" + 2500
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
