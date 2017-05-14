using System;
using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers
{
    public class AnalyseControllerTest
    {
        #region Properties
        private readonly DummyApplicationDbContext _dbContext;
        private readonly Mock<IAnalyseRepository> _analyseRepository;
        private readonly Mock<IJobcoachRepository> _jobcoachRepository;
        private readonly Mock<IExceptionLogRepository> _exceptionLogRepository;
        private readonly AnalyseController _controller;
        #endregion

        #region Constructors
        public AnalyseControllerTest()
        {
            _analyseRepository = new Mock<IAnalyseRepository>();
            _jobcoachRepository = new Mock<IJobcoachRepository>();
            _exceptionLogRepository = new Mock<IExceptionLogRepository>();
            _controller = new AnalyseController(_analyseRepository.Object, _jobcoachRepository.Object,
                _exceptionLogRepository.Object) {TempData = new Mock<ITempDataDictionary>().Object};

            _dbContext = new DummyApplicationDbContext();
            _dbContext.Thomas.Analyses.Add(new Analyse { AnalyseId = 1 });
        }
        #endregion

        #region NieuweAnalyse
        [Fact]
        public void TestNieuweAnalyse_JobcoachNull_MethodeFaaltNiet()
        {
            var result = _controller.NieuweAnalyse(null) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestNieuweAnalyse_RepositoryGooitException_MethodeFaaltNiet()
        {
            _analyseRepository.Setup(r => r.Save()).Throws(new ArgumentException("Exception"));

            var result = _controller.NieuweAnalyse(_dbContext.Thomas) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestNieuweAnalyse_Succes()
        {
            _jobcoachRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(_dbContext.Thomas);

            var result = _controller.NieuweAnalyse(_dbContext.Thomas) as RedirectToActionResult;

            Assert.Equal("SelecteerWerkgever", result?.ActionName);
            Assert.Equal("Werkgever", result?.ControllerName);
        }
        #endregion

        #region OpenAnalyse
        [Fact]
        public void TestOpenAnalyse_RepositoryGooitException_MethodeFaaltNiet()
        {
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.OpenAnalyse(_dbContext.Thomas, 1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestOpenAnalyse_GeenAnalyseVanJobcoach()
        {
            var result = _controller.OpenAnalyse(_dbContext.Thomas, 10) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestOpenAnalyse_AnalyseVerwijderd()
        {
            _dbContext.Thomas.Analyses.Add(new Analyse{AnalyseId = 2, Verwijderd = true});
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(new Analyse {Verwijderd = true});

            var result = _controller.OpenAnalyse(_dbContext.Thomas, 2) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestOpenAnalyse_Succes()
        {
            _analyseRepository.Setup(r => r.GetById(1)).Returns(new Analyse());

            var result = _controller.OpenAnalyse(_dbContext.Thomas, 1) as RedirectToActionResult;


            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Resultaat", result?.ControllerName);
        }
        #endregion

        #region VerwijderAnalyse -- GET --
        [Fact]
        public void TestVerwijderAnalyse_RepositoryGooitException_MethodeFaaltNiet()
        {
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.VerwijderAnalyse(_dbContext.Thomas, 1, "") as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestVerwijderAnalyseGET_GeenAnalyseVanJobcoach()
        {
            var result = _controller.VerwijderAnalyse(_dbContext.Thomas, 10, "") as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestVerwijderAnalyseGET_AnalyseVerwijderd()
        {
            _dbContext.Thomas.Analyses.Add(new Analyse { AnalyseId = 2, Verwijderd = true });
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(new Analyse { Verwijderd = true });

            var result = _controller.VerwijderAnalyse(_dbContext.Thomas, 2, "") as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestVerwijderAnalyse_Succes()
        {
            _analyseRepository.Setup(r => r.GetById(1)).Returns(new Analyse());
            _dbContext.Thomas.Analyses.Add(new Analyse { AnalyseId = 1 });

            var result = _controller.VerwijderAnalyse(_dbContext.Thomas, 1, "") as ViewResult;

            //default view = naam van methode, niet automatisch ingevuld
            Assert.Equal(null, result?.ViewName);
        }
        #endregion

        #region VerwijderAnalyse -- POST --
        [Fact]
        public void TestVerwijderAnalysePOST_RepositoryGooitException_MethodeFaaltNiet()
        {
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.VerwijderAnalyseBevestigd(_dbContext.Thomas, 1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestVerwijdeAnalysePOST_GeenAnalyseVanJobcoach()
        {
            var result = _controller.VerwijderAnalyseBevestigd(_dbContext.Thomas, 10) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestVerwijderAnalysePOST_AnalyseVerwijderd()
        {
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(new Analyse { Verwijderd = true });

            var result = _controller.VerwijderAnalyseBevestigd(_dbContext.Thomas, 1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestVerwijderAnalysePOST_Succes()
        {
            _analyseRepository.Setup(r => r.GetById(1)).Returns(new Analyse());

            var result = _controller.VerwijderAnalyseBevestigd(_dbContext.Thomas, 1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }
        #endregion

        #region Archiveer -- GET --
        [Fact]
        public void TestArchiveer_RepositoryGooitException_MethodeFaaltNiet()
        {
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.Archiveer(_dbContext.Thomas, 1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestArchiveerGET_GeenAnalyseVanJobcoach()
        {
            var result = _controller.Archiveer(_dbContext.Thomas, 10) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestArchiveer_AnalyseVerwijderd()
        {
            _dbContext.Thomas.Analyses.Add(new Analyse { AnalyseId = 2, Verwijderd = true });
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(new Analyse { Verwijderd = true });

            var result = _controller.Archiveer(_dbContext.Thomas, 2) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestArchiveer_Succes()
        {
            _analyseRepository.Setup(r => r.GetById(1)).Returns(new Analyse());

            var result = _controller.Archiveer(_dbContext.Thomas, 1) as ViewResult;

            Assert.Equal("ArchiveerAnalyse", result?.ViewName);
        }
        #endregion

        #region Archiveer -- POST --
        [Fact]
        public void TestArchiveerPOST_RepositoryGooitException_MethodeFaaltNiet()
        {
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.ArchiveerBevestigd(_dbContext.Thomas, 1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestArchiveerPOST_GeenAnalyseVanJobcoach()
        {
            var result = _controller.ArchiveerBevestigd(_dbContext.Thomas, 10) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestArchiveerBevestigd_AnalyseVerwijderd()
        {
            _dbContext.Thomas.Analyses.Add(new Analyse { AnalyseId = 2, Verwijderd = true });
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(new Analyse { Verwijderd = true });

            var result = _controller.ArchiveerBevestigd(_dbContext.Thomas, 2) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestArchiveerPOST_Succes()
        {
            _analyseRepository.Setup(r => r.GetById(1)).Returns(new Analyse());

            var result = _controller.ArchiveerBevestigd(_dbContext.Thomas, 1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }
        #endregion
    }
}
