using System;
using System.Security.Claims;
using System.Threading.Tasks;
using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Models;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.ProfielViewModels;
using KairosWeb_Groep6.Tests.Data;
using KairosWeb_Groep6.Tests.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using Xunit;

namespace KairosWeb_Groep6.Tests.Controllers
{
    public class ProfielControllerTest
    {
        #region Properties
        private readonly Mock<IJobcoachRepository> _jobcoachRepository;
        private readonly ProfielController _controller;
        private readonly DefaultHttpContext _httpctx;
        private readonly DummyApplicationDbContext _dbContext;
        private readonly Mock<FakeUserManager> _userManager;
        private readonly Mock<FakeSignInManager> _signInManager;
        #endregion

        #region Constructors
        public ProfielControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _jobcoachRepository = new Mock<IJobcoachRepository>();
            _userManager = new Mock<FakeUserManager>();
            _signInManager = new Mock<FakeSignInManager>();

            _controller = new ProfielController(_userManager.Object, _jobcoachRepository.Object,
                _signInManager.Object);
            _httpctx = new DefaultHttpContext();
            _controller.ControllerContext.HttpContext = _httpctx;
            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }
        #endregion

        #region Index
        [Fact]
        public void TestIndex()
        {
            _jobcoachRepository.Setup(j => j.GetByEmail(It.IsAny<string>())).Returns(_dbContext.Thomas);

            var result = _controller.Index() as ViewResult;
            var model = result?.Model as ProfielViewModel;

            Assert.Equal(_dbContext.Thomas.Naam, model?.Naam);
            Assert.Equal(_dbContext.Thomas.Voornaam, model?.Voornaam);
            Assert.Equal(_dbContext.Thomas.Emailadres, model?.Email);
            Assert.Equal(_dbContext.Thomas.Organisatie.Naam, model?.OrganisatieNaam);
            Assert.Equal(_dbContext.Thomas.Organisatie.Straat, model?.StraatOrganisatie);
            Assert.Equal(_dbContext.Thomas.Organisatie.Nummer, model?.NrOrganisatie);
            Assert.Equal(_dbContext.Thomas.Organisatie.Gemeente, model?.Gemeente);
        }
        #endregion

        #region Opslaan
        [Fact]
        public async void TestOpslaan_ModelStateInvalid_RedirectsToIndex()
        {
            _controller.ModelState.AddModelError("", "Error");

            var result = await _controller.Opslaan(new ProfielViewModel()) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public async void TestOpslaan_RepositoryGooitException_RedirectsToIndex()
        {
            _jobcoachRepository.Setup(j => j.GetById(It.IsAny<int>())).Throws(new Exception());

            var result = await _controller.Opslaan(new ProfielViewModel()) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);
        }

        [Fact]
        public async void TestOpslaan_EmailNietGewijzigd_RedirectsToIndex()
        {
            _jobcoachRepository.Setup(j => j.GetById(It.IsAny<int>())).Returns(_dbContext.Thomas);

            ProfielViewModel model = new ProfielViewModel(_dbContext.Thomas);
            model.Email = null; // null omdat we HttpContext.User.Identity.Name niet kunnen instellen

            var result = await _controller.Opslaan(model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _jobcoachRepository.Verify(j => j.GetById(_dbContext.Thomas.PersoonId), Times.Once);
            _jobcoachRepository.Verify(j => j.Save(), Times.Once);
        }

        [Fact]
        public async void TestOpslaan_EmailGewijzigd_RedirectsToIndex()
        {
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(() => Task.FromResult(new ApplicationUser()));
            _jobcoachRepository.Setup(j => j.GetById(It.IsAny<int>())).Returns(_dbContext.Thomas);

            ProfielViewModel model = new ProfielViewModel(_dbContext.Thomas);
            model.Email = "iets anders";

            var result = await _controller.Opslaan(model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _userManager.Verify(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()), Times.Once);
            _userManager.Verify(u => u.GenerateChangeEmailTokenAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()), Times.Once);
            _userManager.Verify(u => u.ChangeEmailAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()),
                Times.Once);
            _userManager.Verify(u => u.UpdateAsync(It.IsAny<ApplicationUser>()), Times.Once);

            _signInManager.Verify(s => s.SignOutAsync(), Times.Once);

            _jobcoachRepository.Verify(j => j.GetById(_dbContext.Thomas.PersoonId), Times.Once);
            _jobcoachRepository.Verify(j => j.Save(), Times.Once);
        }
        #endregion
    }
}
