using System;
using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers
{
    public class ContactPersoonControllerTest
    {
        #region Properties
        private readonly Mock<IAnalyseRepository> _analyseRepository;
        private readonly Mock<IDepartementRepository> _departementRepository;
        private readonly Mock<IWerkgeverRepository> _werkgeverRepository;
        private readonly Mock<IContactPersoonRepository> _contactPersoonRepository;
        private readonly Mock<IExceptionLogRepository> _exceptionLogRepository;
        private readonly ContactPersoonController _controller;
        private readonly Mock<Analyse> _analyse;
        private readonly DummyApplicationDbContext _dbContext;
        #endregion

        #region Constructors
        public ContactPersoonControllerTest()
        {
            _analyseRepository = new Mock<IAnalyseRepository>();
            _departementRepository = new Mock<IDepartementRepository>();
            _werkgeverRepository = new Mock<IWerkgeverRepository>();
            _contactPersoonRepository = new Mock<IContactPersoonRepository>();
            _dbContext = new DummyApplicationDbContext();
            _exceptionLogRepository = new Mock<IExceptionLogRepository>();

            _controller = new ContactPersoonController(_analyseRepository.Object,
                _departementRepository.Object, _werkgeverRepository.Object, _contactPersoonRepository.Object,
                _exceptionLogRepository.Object) {TempData = new Mock<ITempDataDictionary>().Object};

            _analyse = new Mock<Analyse>();
        }
        #endregion

        #region Index
        [Fact]
        public void TestIndex_DepartementNull_RedirectNaarWerkgeverIndex()
        {
            var result = _controller.Index(new Analyse()) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Werkgever", result?.ControllerName);
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

        [Fact]
        public void TestIndex_RepositoryGooitException_RedirectNaarWerkgeverIndex()
        {
            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.Index(new Analyse{Departement = _dbContext.Aldi}) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Werkgever", result?.ControllerName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestIndex_ContactPersoonReedsGeselecteerd_ReturnedIndexView()
        {
            _departementRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(_dbContext.Aldi);
            ContactPersoon cp = new ContactPersoon("Thomas", "Aelbrecht", "thomas@test.com");
            ContactPersoonViewModel model = new ContactPersoonViewModel(cp, 0);

            Analyse analyse = new Analyse { Departement = _dbContext.Aldi};

            var result = _controller.Index(analyse) as ViewResult;
            var resultModel = result?.Model as ContactPersoonViewModel;

            Assert.Equal("Index", result?.ViewName);
            Assert.Equal(model.AnalyseId, resultModel?.AnalyseId);
            Assert.Equal(model.Email, resultModel?.Email);
            Assert.Equal(model.Naam, resultModel?.Naam);
            Assert.Equal(model.PersoonId, resultModel?.PersoonId);
            Assert.Equal(model.Voornaam, resultModel?.Voornaam);
            Assert.Equal(model.WerkgeverId, resultModel?.WerkgeverId);
        }

        [Fact]
        public void TestIndex_GeenContactPersoonIngesteld_RedirectNaarVoegContactPersoonToe()
        {
            Analyse analyse = new Analyse { Departement = _dbContext.Aldi };
            Werkgever werkgever = new Werkgever();

            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Returns(werkgever);

            var result = _controller.Index(analyse) as RedirectToActionResult;

            Assert.Equal("VoegContactPersoonToe", result?.ActionName);
        }
        #endregion

        #region VoegToe -- GET --
        [Fact]
        public void TestVoegToe()
        {
            var result = _controller.VoegContactPersoonToe(1) as ViewResult;
            var model = result?.Model as ContactPersoonViewModel;

            Assert.Equal(1, model?.WerkgeverId);
            Assert.Equal("Index", result?.ViewName);
        }
        #endregion

        #region VoegToe -- POST --
        [Fact]
        public void TestVoegToe_RepositoryGooitException_ReturnViewWithModel()
        {
            Analyse analyse = new Analyse
            {
                Departement = _dbContext.Aldi
            };

            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Throws(new Exception());
            ContactPersoonViewModel model = new ContactPersoonViewModel
            {
                AnalyseId = 1,
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Email = "iets@voorbeeld.be"
            };

            var result = _controller.VoegContactPersoonToe(analyse, model) as ViewResult;
            var resultModel = result?.Model as ContactPersoonViewModel;

            Assert.Equal(model.Naam, resultModel?.Naam);
            Assert.Equal(model.Voornaam, resultModel?.Voornaam);
            Assert.Equal(model.Email, resultModel?.Email);
            Assert.Equal(model.AnalyseId, resultModel?.AnalyseId);
            Assert.Equal("Index", result?.ViewName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestVoegToe_Succes()
        {
            Analyse analyse = new Analyse
            {
                Departement = _dbContext.Aldi
            };

            _departementRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(_dbContext.Aldi);

            ContactPersoonViewModel model = new ContactPersoonViewModel
            {
                AnalyseId = 1,
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Email = "iets@voorbeeld.be"
            };

            var result = _controller.VoegContactPersoonToe(analyse, model) as RedirectToActionResult;

            _departementRepository.Verify(c => c.Save(), Times.Once);
            _analyseRepository.Verify(c => c.Save(), Times.Once);

            Assert.Equal("Index", result?.ActionName);
        }
        #endregion

        #region Bewerk -- POST --
        [Fact]
        public void TestBewerkPOST_RepositoryGooitException_ReturnViewWithModel()
        {
            _contactPersoonRepository.Setup(c => c.GetById(It.IsAny<int>())).Throws(new Exception());

            ContactPersoonViewModel expectedModel = new ContactPersoonViewModel
            {
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Email = "iets@voorbeeld.be",
                WerkgeverId = 0,
                PersoonId = 0
            };

            var result = _controller.Opslaan(expectedModel) as ViewResult;
            var resultModel = result?.Model as ContactPersoonViewModel;

            Assert.Equal("Index", result?.ViewName);
            Assert.Equal(expectedModel.Naam, resultModel?.Naam);
            Assert.Equal(expectedModel.Voornaam, resultModel?.Voornaam);
            Assert.Equal(expectedModel.Email, resultModel?.Email);
            Assert.Equal(expectedModel.WerkgeverId, resultModel?.WerkgeverId);
            Assert.Equal(expectedModel.PersoonId, resultModel?.PersoonId);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestBewerkPOST_ContactPersoonNull_ReturnViewWithModel()
        {
            _contactPersoonRepository.Setup(c => c.GetById(It.IsAny<int>())).Returns(() => null);

            ContactPersoonViewModel expectedModel = new ContactPersoonViewModel
            {
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Email = "iets@voorbeeld.be",
                WerkgeverId = 0,
                PersoonId = 0
            };

            var result = _controller.Opslaan(expectedModel) as ViewResult;
            var resultModel = result?.Model as ContactPersoonViewModel;

            Assert.Equal("Index", result?.ViewName);
            Assert.Equal(expectedModel.Naam, resultModel?.Naam);
            Assert.Equal(expectedModel.Voornaam, resultModel?.Voornaam);
            Assert.Equal(expectedModel.Email, resultModel?.Email);
            Assert.Equal(expectedModel.WerkgeverId, resultModel?.WerkgeverId);
            Assert.Equal(expectedModel.PersoonId, resultModel?.PersoonId);
        }

        [Fact]
        public void TestBewerkPOST_ModelStateInvalid_ReturnViewWithModel()
        {
            ContactPersoonViewModel expectedModel = new ContactPersoonViewModel
            {
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Email = "iets@voorbeeld.be",
                WerkgeverId = 0,
                PersoonId = 0
            };

            _controller.ModelState.AddModelError("", "Error");

            var result = _controller.Opslaan(expectedModel) as ViewResult;
            var resultModel = result?.Model as ContactPersoonViewModel;

            Assert.Equal("Index", result?.ViewName);
            Assert.Equal(expectedModel.Naam, resultModel?.Naam);
            Assert.Equal(expectedModel.Voornaam, resultModel?.Voornaam);
            Assert.Equal(expectedModel.Email, resultModel?.Email);
            Assert.Equal(expectedModel.WerkgeverId, resultModel?.WerkgeverId);
            Assert.Equal(expectedModel.PersoonId, resultModel?.PersoonId);
        }

        [Fact]
        public void TestBewerkPOST_Succes()
        {
            _contactPersoonRepository.Setup(c => c.GetById(It.IsAny<int>())).Returns(new ContactPersoon());

            ContactPersoonViewModel model = new ContactPersoonViewModel
            {
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Email = "iets@voorbeeld.be",
                WerkgeverId = 0,
                PersoonId = 0
            };

            var result = _controller.Opslaan(model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _contactPersoonRepository.Verify(c => c.Save(), Times.Once);
        }
        #endregion

        #region VerwijderContactPersoon
        [Fact]
        public void TestVerwijderContactPersoon_RepositoryGooitException_RedirectToIndex()
        {
            _contactPersoonRepository.Setup(c => c.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.VerwijderContactpersoon(0, 0) as RedirectToActionResult;
            
            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestVerwijderContactPersoon_Succes()
        {
            ContactPersoon cp = new ContactPersoon
            {
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Emailadres = "iets@voorbeeld.be"
            };

            _dbContext.Aldi.ContactPersoon = new ContactPersoon();

            _contactPersoonRepository.Setup(c => c.GetById(It.IsAny<int>())).Returns(cp);

            var result = _controller.VerwijderContactpersoon(0, 0) as ViewResult;

            Assert.Equal("Verwijder", result?.ViewName);
            Assert.Equal(0, result?.ViewData["contactPersoonId"]);
            Assert.Equal(0, result?.ViewData["werkgeverId"]);
            Assert.Equal("Thomas Aelbrecht", result?.ViewData["contactpersoon"]);

        }
        #endregion

        #region VerwijderBevestigd
        [Fact]
        public void TestVerwijderBevestigd_RepositoryGooitException_ToontAlleContactPersonen()
        {
            _contactPersoonRepository.Setup(c => c.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.VerwijderBevestigd(0, 0, _analyse.Object) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestVerwijderBevestigd_Succes()
        {
            Analyse analyse = new Analyse {Departement = _dbContext.Aldi};

            ContactPersoon cp = new ContactPersoon
            {
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Emailadres = "iets@voorbeeld.be"
            };

            _contactPersoonRepository.Setup(c => c.GetById(It.IsAny<int>())).Returns(cp);

            var result = _controller.VerwijderBevestigd(0, 0, analyse) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _contactPersoonRepository.Verify(c => c.Remove(cp), Times.Once);
            _contactPersoonRepository.Verify(c => c.Save(), Times.Once);
            _analyseRepository.Verify(c => c.Save(), Times.Once);
        }
        #endregion
    }
}
