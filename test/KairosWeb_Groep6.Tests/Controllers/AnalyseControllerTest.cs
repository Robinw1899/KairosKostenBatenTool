using System;
using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Tests.Data;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers
{
    public class AnalyseControllerTest
    {
        #region Properties
        private readonly Mock<IAnalyseRepository> _analyseRepository;
        private readonly Mock<IJobcoachRepository> _jobcoachRepository;
        private readonly AnalyseController _controller;
        private readonly Jobcoach _jobcoach;
        #endregion

        #region Constructors
        public AnalyseControllerTest()
        {
            _analyseRepository = new Mock<IAnalyseRepository>();
            _jobcoachRepository = new Mock<IJobcoachRepository>();
            _controller = new AnalyseController(_analyseRepository.Object, _jobcoachRepository.Object);

            DummyApplicationDbContext dbContext = new DummyApplicationDbContext();
            _jobcoach = dbContext.Thomas;
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
            _analyseRepository.Setup(r => r.Add(It.IsAny<Analyse>())).Throws(new Exception());

            var result = _controller.NieuweAnalyse(_jobcoach) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestNieuweAnalyse_Succes()
        {
            var result = _controller.NieuweAnalyse(_jobcoach) as RedirectToActionResult;

            Assert.Equal("SelecteerWerkgever", result?.ActionName);
            Assert.Equal("Werkgever", result?.ControllerName);
        }
        #endregion

        #region OpenAnalyse
        [Fact]
        public void TestOpenAnalyse_RepositoryGooitException_MethodeFaaltNiet()
        {
            _analyseRepository.Setup(r => r.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = _controller.OpenAnalyse(1) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }

        [Fact]
        public void TestOpenAnalyse_Succes()
        {
            _analyseRepository.Setup(r => r.GetById(1)).Returns(new Analyse());

            var result = _controller.OpenAnalyse(1) as RedirectToActionResult;


            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Resultaat", result?.ControllerName);
        }
        #endregion
    }
}
