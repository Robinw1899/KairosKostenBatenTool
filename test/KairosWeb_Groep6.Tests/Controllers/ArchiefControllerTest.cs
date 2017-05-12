using System;
using System.Collections.Generic;
using System.Linq;
using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers
{
    public class ArchiefControllerTest
    {
        #region Properties
        private readonly ArchiefController _controller;
        private readonly Mock<IAnalyseRepository> _analyseRepository;
        private readonly Mock<IJobcoachRepository> _jobcoachRepository;
        private readonly DummyApplicationDbContext _dbContext;
        private readonly Analyse _analyseAldi;
        private readonly Analyse _analyse;
        #endregion

        #region Constructors
        public ArchiefControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _analyseRepository = new Mock<IAnalyseRepository>();
            _jobcoachRepository = new Mock<IJobcoachRepository>();
            _analyse = new Analyse {AnalyseId = 2, InArchief = true};
            _analyseAldi = new Analyse {AnalyseId = 1, Departement = _dbContext.Aldi, InArchief = true};

            _controller =
                new ArchiefController(_analyseRepository.Object, _jobcoachRepository.Object)
                {
                    TempData = new Mock<ITempDataDictionary>().Object
                };
        }
        #endregion

        #region Index
        [Fact]
        public void TestIndex_Succes_ReturnsViewWithModel()
        {
            var result = _controller.Index() as ViewResult;
            var model = result?.Model as IndexViewModel;

            Assert.Equal("Index", result?.ViewName);
            Assert.Equal(0, model?.BeginIndex);
            Assert.Equal(9, model?.EindIndex);
        }
        #endregion

        #region VolgendeAnalyses
        [Fact]
        public void TestVolgende_RedirectsToHaalAnalyseOp()
        {
            var result = _controller.Volgende(0, 9) as RedirectToActionResult;
            RouteValueDictionary model = result?.RouteValues;

            Assert.Equal("HaalAnalysesOp", result?.ActionName);
            if (model != null)
            {
                Assert.Equal(9, model["BeginIndex"]);
                Assert.Equal(18, model["EindIndex"]);
            }
        }
        #endregion

        #region VorigeAnalyses
        [Fact]
        public void TestVorige_RedirectsToHaalAnalyseOp()
        {
            var result = _controller.Vorige(9, 18) as RedirectToActionResult;
            RouteValueDictionary model = result?.RouteValues;

            Assert.Equal("HaalAnalysesOp", result?.ActionName);
            if (model != null)
            {
                Assert.Equal(0, model["BeginIndex"]);
                Assert.Equal(9, model["EindIndex"]);
            }
        }
        #endregion

        #region ZoekAnalyse
        [Fact]
        public void TestZoek_RepoGooitException_RedirectToIndex()
        {
            List<Analyse> analyses = new List<Analyse>
            {
                _analyseAldi,
                _analyseAldi,
                _analyseAldi,
                _analyseAldi
            };
            _dbContext.Thomas.Analyses = analyses;

            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.Zoek(_dbContext.Thomas, "verk") as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestZoek_Succes_ReturnsPartial()
        {
            _jobcoachRepository.Setup(r => r.GetByEmail(It.IsAny<string>())).Returns(_dbContext.Thomas);

            var result = _controller.Zoek(_dbContext.Thomas, "hallo") as PartialViewResult;

            Assert.Equal("_Analyses", result?.ViewName);
        }

        [Fact]
        public void TestZoek_Succes_ZoekenOpDepartement_ReturnsIndexViewModel()
        {
            List<Analyse> analyses = new List<Analyse>
            {
                _analyseAldi,
                _analyseAldi,
                _analyseAldi,
                _analyseAldi
            };
            _dbContext.Thomas.Analyses = analyses;

            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns(_analyseAldi);

            var result = _controller.Zoek(_dbContext.Thomas, "verk") as PartialViewResult;
            var model = result?.ViewData.Model as IndexViewModel;

            Assert.Equal(4, model?.Analyses.Count());
        }

        [Fact]
        public void TestZoek_Succes_ZoekenOpWerkgever_ReturnsIndexViewModel()
        {
            List<Analyse> analyses = new List<Analyse>
            {
                _analyseAldi,
                _analyseAldi,
                _analyse,
                _analyse
            };
            _dbContext.Thomas.Analyses = analyses;

            _analyseRepository.Setup(r => r.GetById(1))
                .Returns(_analyseAldi);
            _analyseRepository.Setup(r => r.GetById(2))
                .Returns(_analyse);

            var result = _controller.Zoek(_dbContext.Thomas, "Aldi") as PartialViewResult;
            var model = result?.ViewData.Model as IndexViewModel;

            Assert.Equal(2, model?.Analyses.Count());
        }

        [Fact]
        public void TestZoek_Succes_ZoekenOpGemeente_ReturnsIndexViewModel()
        {
            List<Analyse> analyses = new List<Analyse>
            {
                _analyseAldi,
                _analyseAldi,
                _analyse,
                _analyse
            };
            _dbContext.Thomas.Analyses = analyses;

            _analyseRepository.Setup(r => r.GetById(1))
                .Returns(_analyseAldi);
            _analyseRepository.Setup(r => r.GetById(2))
                .Returns(_analyse);

            var result = _controller.Zoek(_dbContext.Thomas, "Aldi") as PartialViewResult;
            var model = result?.ViewData.Model as IndexViewModel;

            Assert.Equal(2, model?.Analyses.Count());
        }

        [Fact]
        public void TestZoek_Succes_LegeString_ReturnsIndexViewModel()
        {
            List<Analyse> analyses = new List<Analyse>
            {
                _analyseAldi,
                _analyseAldi,
                _analyse,
                _analyse
            };
            _dbContext.Thomas.Analyses = analyses;

            _analyseRepository.Setup(r => r.GetById(1))
                .Returns(_analyseAldi);
            _analyseRepository.Setup(r => r.GetById(2))
                .Returns(_analyse);

            var result = _controller.Zoek(_dbContext.Thomas, "") as PartialViewResult;
            var model = result?.ViewData.Model as IndexViewModel;

            Assert.Equal(4, model?.Analyses.Count());
        }

        [Fact]
        public void TestZoek_Succes_ZoektermNull_ReturnsIndexViewModel()
        {
            List<Analyse> analyses = new List<Analyse>
            {
                _analyse,
                _analyse,
                _analyse,
                _analyse
            };
            _dbContext.Thomas.Analyses = analyses;

            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>()))
                .Returns(_analyse);

            var result = _controller.Zoek(_dbContext.Thomas, null) as PartialViewResult;
            var model = result?.ViewData.Model as IndexViewModel;

            Assert.Equal(4, model?.Analyses.Count());
        }

        #endregion

        #region HaalAnalyseUitArchief -- GET --
        [Fact]
        public void TestHaalAnalyseUitArchief_RepositoryGooitException_MethodeFaaltNiet()
        {
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.HaalAnalyseUitArchief(1) as RedirectToActionResult;
            
            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestHaalAnalyseUitArchief_Succes()
        {
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(_analyse);

            var result = _controller.HaalAnalyseUitArchief(1) as ViewResult;

            Assert.Equal(1, _controller.ViewData["analyseId"]);
            Assert.Equal(null, _controller.ViewData["werkgever"]); // er is geen Departement ingesteld
            Assert.Equal("HaalAnalyseUitArchief", result?.ViewName);
        }
        #endregion

        #region HaalAnalyseUitArchief -- POST --
        [Fact]
        public void TestHaalAnalyseUitArchiefBevestigd_RepositoryGooitException_MethodeFaaltNiet()
        {
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.HaalAnalyseUitArchiefBevestigd(1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public void TestHaalAnalyseUitArchiefBevestigd_Succes()
        {
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Returns(_analyse);

            var result = _controller.HaalAnalyseUitArchiefBevestigd(1) as RedirectToActionResult;

            Assert.False(_analyse.InArchief);
            Assert.Equal("Index", result?.ActionName);
        }
        #endregion

        #region OpenAnalyse
        [Fact]
        public void TestOpenAnalyse()
        {
            var result = _controller.OpenAnalyse(1) as RedirectToActionResult;

            Assert.Equal("OpenAnalyse", result?.ActionName);
            Assert.Equal("Analyse", result?.ControllerName);
        }
        #endregion

        #region VerwijderAnalyse
        [Fact]
        public void TestVerwijderAnalyse()
        {
            var result = _controller.VerwijderAnalyse(1) as RedirectToActionResult;

            Assert.Equal("VerwijderAnalyse", result?.ActionName);
            Assert.Equal("Analyse", result?.ControllerName);
        }
        #endregion

        #region MaakExcelAnalyse
        [Fact]
        public void TestMaakExcelAnalyse()
        {
            var result = _controller.MaakExcelAnalyse(1) as RedirectToActionResult;

            Assert.Equal("MaakExcel", result?.ActionName);
            Assert.Equal("Resultaat", result?.ControllerName);
        }
        #endregion

        #region MailAnalyse
        [Fact]
        public void MailAnalyse()
        {
            var result = _controller.MailAnalyse(1) as RedirectToActionResult;

            Assert.Equal("Mail", result?.ActionName);
            Assert.Equal("Resultaat", result?.ControllerName);
        }
        #endregion

        #region ToonMeer
        [Fact]
        public void TestVolgende9()
        {
            var result = _controller.Volgende9(4) as RedirectToActionResult;
            var aantalSkip = result?.RouteValues["aantalSkip"];

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal(5, aantalSkip);
        }
        #endregion
    }
}
