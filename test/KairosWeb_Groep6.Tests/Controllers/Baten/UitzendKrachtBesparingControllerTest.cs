using System;
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
        private readonly UitzendKrachtBesparingenController _controller;
        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public UitzendKrachtBesparingControllerTest()
        {
            var dbContext = new DummyApplicationDbContext();
            var analyseRepo = new Mock<AnalyseRepository>();

            _controller = new UitzendKrachtBesparingenController(analyseRepo.Object);
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
            UitzendKrachtBesparingViewModel model = new UitzendKrachtBesparingViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
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
        public void TestVerwijder_Succes_RedirectsToIndex()
        {
            var result = _controller.Verwijder(_analyse, 1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }
        #endregion
    }
}
