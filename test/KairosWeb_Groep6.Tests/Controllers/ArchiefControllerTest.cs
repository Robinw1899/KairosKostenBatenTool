using System;
using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers
{
    public class ArchiefControllerTest
    {
        #region Properties
        private readonly ArchiefController _controller;
        private readonly Mock<IAnalyseRepository> _analyseRepository;
        private Analyse _analyse;
        #endregion

        #region Constructors
        public ArchiefControllerTest()
        {
            _analyseRepository = new Mock<IAnalyseRepository>();
            var jobcoachRepository = new Mock<IJobcoachRepository>();
            _analyse = new Analyse();

            _controller = new ArchiefController(_analyseRepository.Object, jobcoachRepository.Object);
            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index
        // To do
        #endregion

        #region VolgendeAnalyses
        // To do
        #endregion

        #region VorigeAnalyses
        // To do
        #endregion

        #region ZoekAnalyse
        // To do
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

        #region MaakPdfAnalyse
        [Fact(Skip = "Not implemented yet")]
        public void MaakPdfAnalyse()
        {
            var result = _controller.MaakPdfAnalyse(1);
        }
        #endregion

        #region AfdrukkenAnalyse
        [Fact(Skip = "Not implemented yet")]
        public void AfdrukkenAnalyse()
        {
            var result = _controller.AfdrukkenAnalyse(1);
        }
        #endregion

        #region ToonMeer
        [Fact]
        public void TestVolgende9()
        {
            var result = _controller.Volgende9(4) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }
        #endregion
    }
}
