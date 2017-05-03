using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Models.AccountViewModels;
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

            _controller = new ContactPersoonController(_analyseRepository.Object,
                _departementRepository.Object, _werkgeverRepository.Object, _contactPersoonRepository.Object);
            _controller.TempData = new Mock<ITempDataDictionary>().Object;

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
        public void TestIndex_RepositoryGooitException_RedirectNaarWerkgeverIndex()
        {
            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.Index(new Analyse()) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Werkgever", result?.ControllerName);
        }

        [Fact]
        public void TestIndex_ContactPersoonReedsGeselecteerd_ReturnedBewerkView()
        {
            ContactPersoon cp = new ContactPersoon("Thomas", "Aelbrecht", "iets@voorbeeld.be");
            ContactPersoonViewModel model = new ContactPersoonViewModel(cp, 0);

            Analyse analyse = new Analyse {ContactPersooon = cp, Departement = _dbContext.Aldi};

            var result = _controller.Index(analyse) as ViewResult;
            var resultModel = result?.Model as ContactPersoonViewModel;

            Assert.Equal("Bewerk", result?.ViewName);
            Assert.Equal(model.AnalyseId, resultModel?.AnalyseId);
            Assert.Equal(model.Email, resultModel?.Email);
            Assert.Equal(model.Naam, resultModel?.Naam);
            Assert.Equal(model.PersoonId, resultModel?.PersoonId);
            Assert.Equal(model.Voornaam, resultModel?.Voornaam);
            Assert.Equal(model.WerkgeverId, resultModel?.WerkgeverId);
        }

        [Fact]
        public void TestIndex_GeenContactPersoonNietIngesteld_ToontAlleContactpersonen()
        {
            Analyse analyse = new Analyse { Departement = _dbContext.Aldi };
            ContactPersoon cp = new ContactPersoon("Thomas", "Aelbrecht", "iets@voorbeeld.be");
            Werkgever werkgever = new Werkgever
            {
                ContactPersonen = new List<ContactPersoon>
                {
                    cp
                }
            };

            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Returns(werkgever);

            var result = _controller.Index(analyse) as RedirectToActionResult;

            Assert.Equal("ToonAlleContactPersonen", result?.ActionName);
        }

        [Fact]
        public void TestIndex_GeenContactPersonenInWerkgever_VoegContactPersoonToe()
        {
            Analyse analyse = new Analyse { Departement = _dbContext.Aldi };
            Werkgever werkgever = new Werkgever{ContactPersonen = new List<ContactPersoon>()};

            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Returns(werkgever);

            var result = _controller.Index(analyse) as RedirectToActionResult;

            Assert.Equal("VoegContactPersoonToe", result?.ActionName);
        }
        #endregion

        #region ToonAlleContactPersonen
        [Fact]
        public void TestToonAlleContactPersonen_RepositoryGooitException_RedirectNaarWerkgeverIndex()
        {
            Analyse analyse = new Analyse {Departement = _dbContext.Aldi};
            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.ToonAlleContactPersonen(analyse) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Werkgever", result?.ControllerName);
        }

        [Fact]
        public void TestToonAlleContactPersonen_Succes()
        {
            ContactPersoon cp = new ContactPersoon("Thomas", "Aelbrecht", "iets@voorbeeld.be");
            Analyse analyse = new Analyse { Departement = _dbContext.Aldi };
            analyse.Departement.Werkgever.ContactPersonen = new List<ContactPersoon> { cp };

            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Returns(analyse.Departement.Werkgever);

            var result = _controller.ToonAlleContactPersonen(analyse) as ViewResult;
            var model = result?.Model as IEnumerable<ContactPersoonViewModel>;

            Assert.Equal("Index", result?.ViewName);
            Assert.Equal(1, model.Count());
        }
        #endregion

        #region VoegToe -- GET --
        [Fact]
        public void TestVoegToe()
        {
            var result = _controller.VoegContactPersoonToe(1) as ViewResult;
            var model = result?.Model as ContactPersoonViewModel;

            Assert.Equal(1, model?.WerkgeverId);
        }
        #endregion

        #region VoegToe -- POST --
        [Fact]
        public void TestVoegToe_RepositoryGooitException_ReturnViewWithModel()
        {
            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Throws(new Exception());
            ContactPersoonViewModel model = new ContactPersoonViewModel
            {
                AnalyseId = 1,
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Email = "iets@voorbeeld.be"
            };

            var result = _controller.VoegContactPersoonToe(model) as ViewResult;
            var resultModel = result?.Model as ContactPersoonViewModel;

            Assert.Equal(model.Naam, resultModel?.Naam);
            Assert.Equal(model.Voornaam, resultModel?.Voornaam);
            Assert.Equal(model.Email, resultModel?.Email);
            Assert.Equal(model.AnalyseId, resultModel?.AnalyseId);
        }

        [Fact]
        public void TestVoegToe_Succes()
        {
            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Returns(_dbContext.Aldi.Werkgever);
            ContactPersoonViewModel model = new ContactPersoonViewModel
            {
                AnalyseId = 1,
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Email = "iets@voorbeeld.be"
            };

            var result = _controller.VoegContactPersoonToe(model) as RedirectToActionResult;

            _contactPersoonRepository.Verify(c => c.Add(It.IsAny<ContactPersoon>()), Times.Once);
            _contactPersoonRepository.Verify(c => c.Save(), Times.Once);

            _werkgeverRepository.Verify(w => w.Save(), Times.Once);

            Assert.Equal("ToonAlleContactPersonen", result?.ActionName);
        }
        #endregion

        #region Bewerk -- GET --
        [Fact]
        public void TestBewerkGET_RepositoryGooitException_RedirectToIndex()
        {
            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.Bewerk(0, 0) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestBewerkGET_Succes()
        {
            ContactPersoon cp = new ContactPersoon("Thomas", "Aelbrecht", "iets@voorbeeld.be");
            _dbContext.Aldi.Werkgever.ContactPersonen = new List<ContactPersoon> { cp };

            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Returns(_dbContext.Aldi.Werkgever);
            ContactPersoonViewModel expectedModel = new ContactPersoonViewModel
            {
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Email = "iets@voorbeeld.be",
                WerkgeverId = 0,
                PersoonId = 0
            };

            var result = _controller.Bewerk(0, 0) as ViewResult;
            var resultModel = result?.Model as ContactPersoonViewModel;

            Assert.Equal(expectedModel.Naam, resultModel?.Naam);
            Assert.Equal(expectedModel.Voornaam, resultModel?.Voornaam);
            Assert.Equal(expectedModel.Email, resultModel?.Email);
            Assert.Equal(expectedModel.WerkgeverId, resultModel?.WerkgeverId);
            Assert.Equal(expectedModel.PersoonId, resultModel?.PersoonId);
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

            var result = _controller.Bewerk(expectedModel) as ViewResult;
            var resultModel = result?.Model as ContactPersoonViewModel;

            Assert.Equal(expectedModel.Naam, resultModel?.Naam);
            Assert.Equal(expectedModel.Voornaam, resultModel?.Voornaam);
            Assert.Equal(expectedModel.Email, resultModel?.Email);
            Assert.Equal(expectedModel.WerkgeverId, resultModel?.WerkgeverId);
            Assert.Equal(expectedModel.PersoonId, resultModel?.PersoonId);

            Assert.Equal(1, _controller.ModelState.ErrorCount); // er is een modelerror toegevoegd
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

            var result = _controller.Bewerk(expectedModel) as ViewResult;
            var resultModel = result?.Model as ContactPersoonViewModel;

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

            var result = _controller.Bewerk(expectedModel) as ViewResult;
            var resultModel = result?.Model as ContactPersoonViewModel;

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

            var result = _controller.Bewerk(model) as RedirectToActionResult;

            Assert.Equal("ToonAlleContactPersonen", result?.ActionName);

            _contactPersoonRepository.Verify(c => c.Save(), Times.Once);
        }
        #endregion

        #region VerwijderContactPersoon
        [Fact]
        public void TestVerwijderContactPersoon_RepositoryGooitException_ToontAlleContactPersonen()
        {
            _contactPersoonRepository.Setup(c => c.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.VerwijderContactpersoon(0, 0, _analyse.Object) as RedirectToActionResult;
            
            Assert.Equal("ToonAlleContactPersonen", result?.ActionName);
        }

        [Fact]
        public void TestVerwijderContactPersoon_ContactpersoonIsHoofdcontactpersoon_ToontAlleContactpersonen()
        {
            ContactPersoon cp = new ContactPersoon
            {
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Emailadres = "iets@voorbeeld.be"
            };

            Analyse analyse = new Analyse {ContactPersooon = cp};
            _contactPersoonRepository.Setup(c => c.GetById(It.IsAny<int>())).Returns(cp);

            var result = _controller.VerwijderContactpersoon(0, 0, analyse) as RedirectToActionResult;

            Assert.Equal("ToonAlleContactPersonen", result?.ActionName);
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

            Analyse analyse = new Analyse { ContactPersooon = new ContactPersoon() };
            _contactPersoonRepository.Setup(c => c.GetById(It.IsAny<int>())).Returns(cp);

            var result = _controller.VerwijderContactpersoon(0, 0, analyse) as ViewResult;

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

            var result = _controller.VerwijderBevestigd(0, 0) as RedirectToActionResult;

            Assert.Equal("ToonAlleContactPersonen", result?.ActionName);
        }

        [Fact]
        public void TestVerwijderBevestigd_Succes()
        {
            ContactPersoon cp = new ContactPersoon
            {
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Emailadres = "iets@voorbeeld.be"
            };

            _dbContext.Aldi.Werkgever.ContactPersonen = new List<ContactPersoon> { cp };
            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Returns(_dbContext.Aldi.Werkgever);
            _contactPersoonRepository.Setup(c => c.GetById(It.IsAny<int>())).Returns(cp);

            var result = _controller.VerwijderBevestigd(0, 0) as RedirectToActionResult;

            Assert.Equal("ToonAlleContactPersonen", result?.ActionName);

            _contactPersoonRepository.Verify(c => c.Remove(cp), Times.Once);
            _contactPersoonRepository.Verify(c => c.Save(), Times.Once);

            _werkgeverRepository.Verify(c => c.Save(), Times.Once);
        }
        #endregion

        #region SelecteerContactPersoon -- met id --
        [Fact]
        public void TestSelecteerContactPersoon_MetId()
        {
            var result = _controller.SelecteerHoofdContactPersoon(0, 0) as RedirectToActionResult;

            Assert.Equal("SelecteerContactPersoon", result?.ActionName);
        }
        #endregion

        #region SelecteerContactPersoon -- met analyse --
        [Fact]
        public void TestSelecteerContactPersoon_RepositoryGooitException_RedirectsToIndex()
        {
            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.SelecteerContactPersoon(0, 0, _analyse.Object) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestSelecteerContactPersoon_MetAnalyse_Succes()
        {
            ContactPersoon cp = new ContactPersoon
            {
                Voornaam = "Thomas",
                Naam = "Aelbrecht",
                Emailadres = "iets@voorbeeld.be"
            };

            _dbContext.Aldi.Werkgever.ContactPersonen = new List<ContactPersoon> { cp };
            _werkgeverRepository.Setup(w => w.GetById(It.IsAny<int>())).Returns(_dbContext.Aldi.Werkgever);

            var result = _controller.SelecteerContactPersoon(0, 0, _analyse.Object) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _analyseRepository.Verify(a => a.Save(), Times.Once);
        }
        #endregion
    }
}
