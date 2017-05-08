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

namespace KairosWeb_Groep6.Tests.Controllers.Kosten
{
    public class LoonkostenControllerTest
    {
        #region Properties
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
            var analyseRepo = new Mock<AnalyseRepository>();
            _doelgroepRepository = new Mock<IDoelgroepRepository>();

            _controller = new LoonkostenController(analyseRepo.Object, _doelgroepRepository.Object);
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
            LoonkostFormViewModel model = new LoonkostFormViewModel();

            var result = _controller.VoegToe(_analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
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
                IBOPremie = "" + 564.0M
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
        public void TestVerwijder_Succes_RedirectsToIndex()
        {
            var result = _controller.Verwijder(_analyse, 1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }
        #endregion
    }
}
