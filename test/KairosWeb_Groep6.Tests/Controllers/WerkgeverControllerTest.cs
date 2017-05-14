using System;
using System.Collections.Generic;
using System.Linq;
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
    public class WerkgeverControllerTest
    {
        #region Properties
        private readonly Mock<IAnalyseRepository> _analyseRepository;
        private readonly Mock<IDepartementRepository> _departementRepository;
        private readonly Mock<IWerkgeverRepository> _werkgeverRepository;
        private readonly Mock<IExceptionLogRepository> _exceptionLogRepository;
        private readonly WerkgeverController _controller;
        private readonly Mock<Analyse> _analyse;
        private readonly DummyApplicationDbContext _dbContext;
        #endregion

        #region Constructors
        public WerkgeverControllerTest()
        {
            _analyseRepository = new Mock<IAnalyseRepository>();
            _departementRepository = new Mock<IDepartementRepository>();
            _werkgeverRepository = new Mock<IWerkgeverRepository>();
            _exceptionLogRepository = new Mock<IExceptionLogRepository>();

            _controller = new WerkgeverController(_analyseRepository.Object,
                _departementRepository.Object, _werkgeverRepository.Object, _exceptionLogRepository.Object)
            {
                TempData = new Mock<ITempDataDictionary>().Object
            };

            _analyse = new Mock<Analyse>();
            _dbContext = new DummyApplicationDbContext();
        }
        #endregion

        #region Index
        [Fact]
        public void TestIndex_DepartementNull_RedirectsToSelecteerWerkgever()
        {
            var result = _controller.Index(new Analyse()) as RedirectToActionResult;

            Assert.Equal("SelecteerWerkgever", result?.ActionName);
        }

        [Fact]
        public void TestIndex_Succes()
        {
            Analyse analyse = new Analyse { Departement = _dbContext.Aldi };

            var result = _controller.Index(analyse) as ViewResult;
            var model = result?.Model as WerkgeverViewModel;

            Assert.Equal(typeof(WerkgeverViewModel), result?.Model.GetType());
            Assert.Equal("ALDI", model?.Naam);
        }
        #endregion

        #region Opslaan
        [Fact]
        public void TestOpslaan_RepositoryGooitException_ReturnsViewWithModel()
        {
            _departementRepository.Setup(d => d.GetById(It.IsAny<int>())).Throws(new Exception());

            WerkgeverViewModel model = new WerkgeverViewModel(_dbContext.Aldi);

            var result = _controller.Opslaan(_analyse.Object, model) as ViewResult;
            var resultModel = result?.Model as WerkgeverViewModel;

            Assert.Equal(model, resultModel);
            Assert.Equal("Index", result?.ViewName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestOpslaan_Succes()
        {
            Analyse analyse = new Analyse { Departement = _dbContext.Aldi };

            _departementRepository.Setup(d => d.GetById(It.IsAny<int>())).Returns(_dbContext.Aldi);

            WerkgeverViewModel model = new WerkgeverViewModel(_dbContext.Aldi);

            var result = _controller.Opslaan(analyse, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal(_dbContext.Aldi, analyse.Departement);
        }
        #endregion

        #region SelecteerWerkgever
        [Fact]
        public void TestSelecteerWerkgever()
        {
            var result = _controller.SelecteerWerkgever() as ViewResult;
            
            Assert.Equal("SelecteerWerkgever", result?.ViewName);
        }
        #endregion

        #region Nieuwe werkgever -- GET --
        [Fact]
        public void TestNieuweWerkgeverGET_ReturnsViewWithModel()
        {
            var result = _controller.NieuweWerkgever() as ViewResult;
            var model = result?.Model as WerkgeverViewModel;

            Assert.Equal(typeof(WerkgeverViewModel), model?.GetType());
            Assert.Equal(35, model?.PatronaleBijdrage);
        }
        #endregion

        #region Nieuwe werkgever -- POST --
        [Fact]
        public void TestNieuweWerkgever_RepositoryGooitException_MethodeFaaltNiet()
        {
            _departementRepository.Setup(r => r.Save()).Throws(new Exception());

            WerkgeverViewModel model = new WerkgeverViewModel();

            var result = _controller.NieuweWerkgever(_analyse.Object, model) as ViewResult;
            var resultModel = result?.Model as WerkgeverViewModel;

            Assert.Equal(model, resultModel);
            Assert.Equal("NieuweWerkgever", result?.ViewName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestNieuweWerkgever_BestaandDepartement_ReturnsViewMetZelfdeModel()
        {
            _departementRepository.Setup(r => r.GetByName(It.IsAny<string>())).Returns(_dbContext.Aldi);

            WerkgeverViewModel model = new WerkgeverViewModel(_dbContext.Aldi);

            var result = _controller.NieuweWerkgever(_analyse.Object, model) as ViewResult;
            var resultModel = result?.Model as WerkgeverViewModel;

            Assert.Equal(model, resultModel);
            Assert.Equal("NieuweWerkgever", result?.ViewName);
        }

        [Fact]
        public void TestNieuweWerkgever_Succes_RedirectsToIndexContactPersoon()
        {
            _departementRepository.Setup(r => r.GetByName(It.IsAny<string>())).Returns(_dbContext.Aldi);

            WerkgeverViewModel model = new WerkgeverViewModel
            {
                AantalWerkuren = 35M,
                Bus = "",
                Departement = "HR",
                Gemeente = "Wieze",
                Postcode = 9280,
                Nummer = 42,
                Straat = "Wolvenstraat",
                PatronaleBijdrage = 35M,
                Naam = "Thomas NV"
            };

            var result = _controller.NieuweWerkgever(_analyse.Object, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("ContactPersoon", result?.ControllerName);

            _departementRepository.Verify(r => r.Add(It.IsAny<Departement>()), Times.Once);
            _departementRepository.Verify(r => r.Save(), Times.Once);

            _analyseRepository.Verify(r => r.Save(), Times.Once);
        }
        #endregion

        #region Bestaande werkgever -- GET --
        [Fact]
        public void TestBestaandeWerkgever_ReturnsViewWithModel()
        {
            var result = _controller.BestaandeWerkgever() as ViewResult;
            var model = result?.Model as BestaandeWerkgeverViewModel;

            Assert.Equal(0, model?.Werkgevers.Count());
        }
        #endregion

        #region Selecteer bestaande werkgever -- POST --
        [Fact]
        public void TestBestaandeWerkgever_RepositoryGooitException_MethodeFaaltNiet()
        {
            _analyseRepository.Setup(a => a.Save()).Throws(new Exception());

            var result = _controller.SelecteerBestaandeWerkgever(_analyse.Object, 1, 1) as RedirectToActionResult;

            Assert.Equal("BestaandeWerkgever", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestBestaandeWerkgever_Succes_RedirectsToIndexContactPersoon()
        {
            _departementRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(_dbContext.Aldi);

            var result = _controller.SelecteerBestaandeWerkgever(_analyse.Object, 1, 1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("ContactPersoon", result?.ControllerName);

            _analyseRepository.Verify(a => a.Save(), Times.Once);
        }
        #endregion

        #region ZoekDepartement
        [Fact]
        public void TestZoekDepartement_RepositoryGooitException_RedirectsToBestaandDepartement()
        {
            _departementRepository.Setup(r => r.GetAllVanWerkgever(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.ZoekDepartementen(1, "hallo") as RedirectToActionResult;

            Assert.Equal("BestaandDepartement", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestZoekDepartement_ReturnsPartial()
        {
            _departementRepository.Setup(r => r.GetAllVanWerkgever(It.IsAny<int>()))
                .Returns(new List<Departement>());

            var result = _controller.ZoekDepartementen(1, "geen") as PartialViewResult;

            Assert.Equal("_Departementen", result?.ViewName);
        }
        #endregion

        #region ZoekWerkgever
        [Fact]
        public void TestZoekWerkgever_RepositoryGooitException_RedirectsToBestaandeWerkgever()
        {
            _werkgeverRepository.Setup(r => r.GetByName(It.IsAny<string>()))
                .Throws(new Exception());

            var result = _controller.ZoekWerkgever(new BestaandeWerkgeverViewModel(), "hallo") as RedirectToActionResult;

            Assert.Equal("BestaandeWerkgever", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestZoekWerkgever_NaamNull_MethodeFaaltNiet()
        {
            _werkgeverRepository.Setup(r => r.GetAll())
                .Returns(new List<Werkgever>());

            var result = _controller.ZoekWerkgever(null) as PartialViewResult;

            Assert.Equal("_Werkgevers", result?.ViewName);
        }

        [Fact]
        public void TestZoekWerkgever_NaamEmpty_MethodeFaaltNiet()
        {
            _werkgeverRepository.Setup(r => r.GetWerkgevers())
                .Returns(new List<Werkgever>
                {
                    _dbContext.Aldi.Werkgever,
                    _dbContext.Aldi.Werkgever,
                    _dbContext.Aldi.Werkgever
                });

            var result = _controller.ZoekWerkgever(new BestaandeWerkgeverViewModel(), "") as PartialViewResult;
            var model = result?.ViewData.Model as BestaandeWerkgeverViewModel;

            Assert.Equal("_Werkgevers", result?.ViewName);
            Assert.Equal(3, model?.Werkgevers.Count());
        }

        [Fact]
        public void TestZoekWerkgever_GeenResultaten_ReturnsEmptyModel()
        {
            _werkgeverRepository.Setup(r => r.GetWerkgevers())
                .Returns(new List<Werkgever>
                {
                    _dbContext.Aldi.Werkgever,
                    _dbContext.Aldi.Werkgever,
                    _dbContext.Aldi.Werkgever
                });

            var result = _controller.ZoekWerkgever(new BestaandeWerkgeverViewModel(), "geen") as PartialViewResult;
            var model = result?.ViewData.Model as BestaandeWerkgeverViewModel;

            Assert.Equal("_Werkgevers", result?.ViewName);
            Assert.Equal(0, model?.Werkgevers.Count());
        }

        [Fact]
        public void TestZoekWerkgever_Succes_ReturnsPartialWithModel()
        {
            List<Werkgever> werkgevers = new List<Werkgever>
            {
                _dbContext.Aldi.Werkgever,
                _dbContext.Aldi.Werkgever,
                _dbContext.Aldi.Werkgever
            };

            _werkgeverRepository.Setup(r => r.GetWerkgevers())
                .Returns(werkgevers);
            _werkgeverRepository.Setup(r => r.GetByName("aldi"))
                .Returns(werkgevers);

            var result = _controller.ZoekWerkgever(new BestaandeWerkgeverViewModel(), "aldi") as PartialViewResult;
            var model = result?.ViewData.Model as BestaandeWerkgeverViewModel;

            Assert.Equal("_Werkgevers", result?.ViewName);
            Assert.Equal(3, model?.Werkgevers.Count());
        }
        #endregion

        #region Bestaand departement
        [Fact]
        public void TestBestaandDepartement_RepositoryGooitException_RedirectsToBestaandeWerkgever()
        {
            _departementRepository.Setup(d => d.GetAllVanWerkgever(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.BestaandDepartement(1) as RedirectToActionResult;

            Assert.Equal("BestaandeWerkgever", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestBestaandDepartement_Succes()
        {
            _departementRepository.Setup(d => d.GetAllVanWerkgever(It.IsAny<int>()))
                .Returns(new List<Departement>
                {
                    _dbContext.Aldi
                });

            var result = _controller.BestaandDepartement(1) as ViewResult;
            var model = result?.Model as BestaandDepartementViewModel;

            Assert.Equal(1, model?.WerkgeverId);
            Assert.Equal(1, model?.Departementen.Count());
        }
        #endregion

        #region Nieuw departement -- GET --
        [Fact]
        public void TestNieuwDepartementGET_RepositoryGooitException_RedirectsToBestaandeWerkgever()
        {
            _werkgeverRepository.Setup(d => d.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.NieuwDepartement(1) as RedirectToActionResult;

            Assert.Equal("BestaandeWerkgever", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestNieuwDepartementGET_Succes()
        {
            _werkgeverRepository.Setup(d => d.GetById(It.IsAny<int>()))
                .Returns(_dbContext.Aldi.Werkgever);

            var result = _controller.NieuwDepartement(1) as ViewResult;
            var model = result?.Model as WerkgeverViewModel;

            Assert.Equal(0, model?.WerkgeverId);
            Assert.Equal("ALDI", model?.Naam);
        }
        #endregion

        #region Nieuw departement -- POST --
        [Fact]
        public void TestNieuwDepartementPOST_RepositoryGooitException_ReturnsView()
        {
            _departementRepository.Setup(d => d.GetByName(It.IsAny<string>())).Throws(new Exception());

            var result = _controller.NieuwDepartement(_analyse.Object, new WerkgeverViewModel()) as ViewResult;

            Assert.Equal(typeof(WerkgeverViewModel), result?.Model.GetType());

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestNieuwDepartementPOST_BestaandDepartement_ReturnsView()
        {
            _departementRepository.Setup(d => d.GetByName(It.IsAny<string>()))
                .Returns(_dbContext.Aldi);

            WerkgeverViewModel model = new WerkgeverViewModel(_dbContext.Aldi);

            var result = _controller.NieuwDepartement(_analyse.Object, model) as ViewResult;
            var resultModel = result?.Model as WerkgeverViewModel;

            Assert.Equal(typeof(WerkgeverViewModel), resultModel?.GetType());
            Assert.Equal(0, resultModel?.WerkgeverId);
            Assert.Equal("ALDI", resultModel?.Naam);
            Assert.Equal("NieuwDepartement", result?.ViewName);
        }

        [Fact]
        public void TestNieuwDepartementPOST_Succes()
        {
            _werkgeverRepository.Setup(d => d.GetById(It.IsAny<int>()))
                .Returns(_dbContext.Aldi.Werkgever);

            WerkgeverViewModel model = new WerkgeverViewModel(_dbContext.Aldi);

            var result = _controller.NieuwDepartement(_analyse.Object, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Resultaat", result?.ControllerName);
        }
        #endregion
    }
}
