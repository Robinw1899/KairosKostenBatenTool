using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Routing;

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
        private readonly Mock<IExceptionLogRepository> _exceptionLogRepository;

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
            _exceptionLogRepository = new Mock<IExceptionLogRepository>();
            
            _controller = new KairosController(_signManager.Object, _userManager.Object,
                _jobcoachRepository.Object, _analyseRepository.Object, _exceptionLogRepository.Object);
            _controller.TempData = new Mock<ITempDataDictionary>().Object;

            _dbContext.Thomas.Analyses = new List<Analyse>
            {
                new Analyse(),
                new Analyse(),
                new Analyse(),
                new Analyse(),
                new Analyse(),
                new Analyse(),
                new Analyse(),
                new Analyse(),
                new Analyse(),
                new Analyse(),
                new Analyse(),
                new Analyse()
            };
        }
        #endregion

        #region Index
        [Fact]
        public async void TestIndex_UserNull_SignsOutAndRedirectsToLogin()
        {
            user = null;
            _userManager.Setup(u => u.GetUserAsync(null))
                .Returns(() => Task.FromResult(user));

            var result = await _controller.Index() as RedirectToActionResult;

            Assert.Equal("Login", result?.ActionName);
            Assert.Equal("Account", result?.ControllerName);

            _signManager.Verify(s => s.SignOutAsync());
        }

        [Fact]
        public async void TestIndex_Succes_ReturnsViewWithModel()
        {
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(() => Task.FromResult(user));

            var result = await _controller.Index() as ViewResult;
            var model = result?.Model as IndexViewModel;

            Assert.Equal("Index", result?.ViewName);
            Assert.Equal(0, model?.BeginIndex);
            Assert.Equal(9, model?.EindIndex);
        }
        #endregion

        #region HaalAnalysesOpZonderModel
        [Fact]
        public void TestHaalAnalysesOpZonderModel_RedirectsToHaalAnalysesOp()
        {
            var result = _controller.HaalAnalysesOpZonderModel(18, 27) as RedirectToActionResult;
            RouteValueDictionary model = result?.RouteValues;

            Assert.Equal("HaalAnalysesOp", result?.ActionName);
            if (model != null)
            {
                Assert.Equal(18, model["BeginIndex"]);
                Assert.Equal(27, model["EindIndex"]);
            }
        }
        #endregion

        #region HaalAnalysesOp
        [Fact]
        public void TestHaalAnalysesOp_RepoGooitException_RedirectsToIndex()
        {
            _analyseRepository.Setup(r => r.SetAnalysesJobcoach(_dbContext.Thomas, false))
                .Throws(new Exception());
            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = 0,
                EindIndex = 9
            };

            var result = _controller.HaalAnalysesOp(_dbContext.Thomas, model) as RedirectToActionResult;

            Assert.Equal("Index", result?.ActionName);

            _exceptionLogRepository.Verify(r => r.Add(It.IsAny<ExceptionLog>()), Times.Once);
            _exceptionLogRepository.Verify(r => r.Save(), Times.Once);
        }

        [Fact]
        public void TestHaalAnalysesOp_VolgendeTrue()
        {
            _analyseRepository.Setup(r => r.GetAnalyses(_dbContext.Thomas, It.IsAny<int>(), It.IsAny<int>()))
                .Returns(_dbContext.Thomas.Analyses);

            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = 0,
                EindIndex = 9
            };

            var result = _controller.HaalAnalysesOp(_dbContext.Thomas, model) as PartialViewResult;
            var resultModel = result?.ViewData.Model as IndexViewModel;

            Assert.Equal("_Analyses", result?.ViewName);
            Assert.True(resultModel?.ShowVolgende);
        }

        [Fact]
        public void TestHaalAnalysesOp_VorigeTrue()
        {
            _analyseRepository.Setup(r => r.GetAnalyses(_dbContext.Thomas, It.IsAny<int>(), It.IsAny<int>()))
                .Returns(_dbContext.Thomas.Analyses);

            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = 9,
                EindIndex = 18
            };

            var result = _controller.HaalAnalysesOp(_dbContext.Thomas, model) as PartialViewResult;
            var resultModel = result?.ViewData.Model as IndexViewModel;

            Assert.Equal("_Analyses", result?.ViewName);
            Assert.True(resultModel?.ShowVorige);
        }

        [Fact]
        public void TestHaalAnalysesOp_Succes()
        {
            _analyseRepository.Setup(r => r.GetAnalyses(_dbContext.Thomas, It.IsAny<int>(), It.IsAny<int>()))
                .Returns(_dbContext.Thomas.Analyses);

            IndexViewModel model = new IndexViewModel
            {
                BeginIndex = 2,
                EindIndex = 11
            };

            var result = _controller.HaalAnalysesOp(_dbContext.Thomas, model) as PartialViewResult;
            var resultModel = result?.ViewData.Model as IndexViewModel;

            Assert.Equal("_Analyses", result?.ViewName);
            Assert.True(resultModel?.ShowVorige, "ShowVorige was false");
            Assert.True(resultModel?.ShowVolgende, "ShowVolgende was false");
            Assert.Equal(2, resultModel?.BeginIndex);
            Assert.Equal(11, resultModel?.EindIndex);
        }
        #endregion

        #region VolgendeAnalyse
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

        #region VorigeAnalyse
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
            //idResult.Setup(r => r.Succeeded).Returns(false);

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

        [Fact]
        public async void EersteKeerAanmeldenPOST_Succes()
        {
            IdentityResult idResult = IdentityResult.Success;
            Microsoft.AspNetCore.Identity.SignInResult signInResult
                = Microsoft.AspNetCore.Identity.SignInResult.Success;
           
            _userManager.Setup(u => u.ResetPasswordAsync(user, It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(idResult));
            _userManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .Returns(() => Task.FromResult(user));
            _signManager.Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), false, false))
                .Returns(Task.FromResult(signInResult));
            _jobcoachRepository.Setup(j => j.GetByEmail(It.IsAny<string>()))
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
        public async void TestOpmerking_ModelStateInvalid_ReturnsViewWithModel()
        {
            string bericht = "Dit is het bericht";
            string onderwerp = "Dummy onderwerp";
            OpmerkingViewModel model = new OpmerkingViewModel(onderwerp, bericht);

            _controller.ModelState.AddModelError("", "Error");

            var result = await _controller.Opmerking(model) as ViewResult;
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
