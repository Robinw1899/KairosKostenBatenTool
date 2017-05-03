using System;
using System.Security.Claims;
using System.Threading.Tasks;
using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Models;
using Moq;
using Xunit;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using KairosWeb_Groep6.Tests.Data;
using KairosWeb_Groep6.Tests.Managers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace KairosWeb_Groep6.Tests.Controllers
{
    public class KairosControllerTest
    {
        #region Properties
        private readonly KairosController _controller;
        private readonly Mock<IJobcoachRepository> _jobcoachRepository;
        private readonly Mock<IAnalyseRepository> _analyseRepository;
        private readonly DummyApplicationDbContext _dbContext;
        private readonly Mock<FakeUserManager> _userManager;
        private readonly Mock<FakeSignInManager> _signManager;

        private EersteKeerAanmeldenViewModel _aanmeldenViewModel;
        private EersteKeerAanmeldenViewModel _foutAanmeldenViewModel;

        ApplicationUser user = new ApplicationUser { Email = "thomasaelbrecht@live.com", Voornaam = "Thomas" };
        #endregion

        #region Constructor
        public KairosControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _jobcoachRepository = new Mock<IJobcoachRepository>();
            _analyseRepository = new Mock<IAnalyseRepository>();
            _userManager = new Mock<FakeUserManager>();
            _signManager = new Mock<FakeSignInManager>();
            
            _controller = new KairosController(_signManager.Object, _userManager.Object,
                _jobcoachRepository.Object, _analyseRepository.Object);
            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index
        // To do
        #endregion

        #region VolgendeAnalyse
        // To do
        #endregion

        #region VorigeAnalyse
        // To do
        #endregion

        #region ZoekAnalyse
        // To do
        #endregion

        #region EersteKeerAanmelden -- GET --
        [Fact]
        public async void EersteKeerAanmeldenGET_ExceptionGegooid_LogtUit()
        {
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Throws(new Exception());

            var result = await _controller.EersteKeerAanmelden() as RedirectToActionResult;

            Assert.Equal("Login", result?.ActionName);
            Assert.Equal("Account", result?.ControllerName);

            _signManager.Verify(s => s.SignOutAsync(), Times.Once);
        }

        [Fact]
        public async void EersteKeerAanmeldenGET_Succes()
        {
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(() => Task.FromResult(user));
            _jobcoachRepository.Setup(j => j.GetByEmail(It.IsAny<string>()))
                .Returns(_dbContext.Thomas);

            var result = await _controller.EersteKeerAanmelden() as ViewResult;
            var model = result?.Model as EersteKeerAanmeldenViewModel;

            Assert.Equal(user.Email, model?.Email);
            Assert.Equal(_dbContext.Thomas.AlAangemeld, model?.AlAangemeld);
        }
        #endregion

        #region EersteKeerAanmelden -- POST --
        [Fact]
        public async void EersteKeerAanmelden_ResultNotSucceeded_LogUit()
        {
            Mock<IdentityResult> idResult = new Mock<IdentityResult>();
            idResult.Setup(r => r.Succeeded).Returns(false);

            _userManager.Setup(u => u.ResetPasswordAsync(user, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(idResult.Object));
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(() => Task.FromResult(user));

            var result = await _controller.EersteKeerAanmelden(new EersteKeerAanmeldenViewModel()) as RedirectToActionResult;

            Assert.Equal("Login", result?.ActionName);
            Assert.Equal("Account", result?.ControllerName);

            _userManager.Verify(u => u.GeneratePasswordResetTokenAsync(user), Times.Once);
            _signManager.Verify(s => s.SignOutAsync(), Times.Once);
        }

        [Fact(Skip = "Not implemented yet")]
        public async void EersteKeerAanmeldenPOST_Succes()
        {
            Mock<IdentityResult> idResult = new Mock<IdentityResult>();
            idResult.Setup(r => r.Succeeded).Returns(true);

            _userManager.Setup(u => u.ResetPasswordAsync(user, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(idResult.Object));
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(() => Task.FromResult(user));
            _jobcoachRepository.Setup(j => j.GetByEmail(_dbContext.Thomas.Emailadres))
                .Returns(_dbContext.Thomas);

            var result = await _controller.EersteKeerAanmelden(new EersteKeerAanmeldenViewModel()) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);

            Assert.True(_dbContext.Thomas.AlAangemeld);

            _userManager.Verify(u => u.GeneratePasswordResetTokenAsync(user), Times.Once);
            _signManager.Verify(s => s.SignOutAsync(), Times.Once);

            _jobcoachRepository.Verify(j => j.Save(), Times.Once);
        }
        #endregion

        #region Opmerking -- GET --
        [Fact]
        public void TestOpmerkingGET()
        {
            var result = _controller.Opmerking() as ViewResult;
            var model = result?.Model as OpmerkingViewModel;

            Assert.Equal(null, model?.Onderwerp);
            Assert.Equal(null, model?.Bericht);
        }
        #endregion

        #region Opmerking -- POST --
        [Fact]
        public void TestOpmerking_ModelStateInvalid_ReturnsViewWithModel()
        {
            string bericht = "Dit is het bericht";
            string onderwerp = "Dummy onderwerp";
            OpmerkingViewModel model = new OpmerkingViewModel(onderwerp, bericht);

            _controller.ModelState.AddModelError("", "Error");

            var result = _controller.Opmerking() as ViewResult;
            var resultModel = result?.Model as OpmerkingViewModel;

            Assert.Equal(onderwerp, resultModel?.Onderwerp);
            Assert.Equal(bericht, resultModel?.Bericht);
        }

        [Fact]
        public async void TestOpmerking_ExceptionGegooid_ReturnsViewWithModel()
        {
            string bericht = "Dit is het bericht";
            string onderwerp = "Dummy onderwerp";
            OpmerkingViewModel model = new OpmerkingViewModel(onderwerp, bericht);

            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Throws(new Exception());

            var result = await _controller.Opmerking(model) as ViewResult;
            var resultModel = result?.Model as OpmerkingViewModel;

            Assert.Equal(onderwerp, resultModel?.Onderwerp);
            Assert.Equal(bericht, resultModel?.Bericht);
        }

        [Fact]
        public async void TestOpmerking_Succes()
        {
            string bericht = "Dit is het bericht";
            string onderwerp = "Dummy onderwerp";
            OpmerkingViewModel model = new OpmerkingViewModel(onderwerp, bericht);

            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(() => Task.FromResult(user));

            var result = await _controller.Opmerking(model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
            Assert.Equal("Kairos", result?.ControllerName);
        }
        #endregion
    }
}
