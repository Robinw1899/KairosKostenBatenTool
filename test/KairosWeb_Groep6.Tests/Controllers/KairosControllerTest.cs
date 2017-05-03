using KairosWeb_Groep6.Controllers;
using KairosWeb_Groep6.Models;
using Moq;
using Xunit;
using KairosWeb_Groep6.Models.Domain;
using KairosWeb_Groep6.Models.KairosViewModels;
using KairosWeb_Groep6.Tests.Data;
using KairosWeb_Groep6.Tests.Managers;
using Microsoft.AspNetCore.Identity;
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

        private EersteKeerAanmeldenViewModel _aanmeldenViewModel;
        private EersteKeerAanmeldenViewModel _foutAanmeldenViewModel;

        private string Wachtwoord = "test";
        private string FoutWachtwoord = "test2";
        private string Email = "thomas.ae@test.be";
        private string FouteEmail = "dimmy.m@test.be";
        #endregion

        #region Constructor

        public KairosControllerTest()
        {
            _dbContext = new DummyApplicationDbContext();
            _jobcoachRepository = new Mock<IJobcoachRepository>();
            _analyseRepository = new Mock<IAnalyseRepository>();
            
            _aanmeldenViewModel = new EersteKeerAanmeldenViewModel()
            {
                Email = Email,
                Password = Wachtwoord,
                ConfirmPassword = Wachtwoord
            };

            _foutAanmeldenViewModel = new EersteKeerAanmeldenViewModel()
            {
                Email = FouteEmail,
                Password = Wachtwoord,
                ConfirmPassword = FoutWachtwoord
            };
            //_jobcoachThomas = new Jobcoach("Ae", "thomas", Email, new Organisatie());

            _controller = new KairosController(Mock.Of<FakeSignInManager>(), Mock.Of<FakeUserManager>(),
                _jobcoachRepository.Object, _analyseRepository.Object);
            _controller.TempData = new Mock<ITempDataDictionary>().Object;
        }

        #endregion

        #region 1steKeerAanmelden HttpGet
        [Fact]
        public void EersteKeerAanmelden()
        {

        }

        #endregion

        #region 1steKeerAanmelden HttpPost
        [Fact(Skip = "Not implemented yet")]
        public void EersteKeerAanmelden_WachtwoordVeranderdIndienSuccesvol()
        {
            Mock<AccountController> _accountController = new Mock<AccountController>();
            _jobcoachRepository.Setup(m => m.GetByEmail(Email)).Returns(_dbContext.Thomas);
            EersteKeerAanmeldenViewModel EersteKeerAanmeldenVM = new EersteKeerAanmeldenViewModel();
            EersteKeerAanmeldenVM.Password = Wachtwoord;
            EersteKeerAanmeldenVM.ConfirmPassword = Wachtwoord;
            
            Assert.Equal(Wachtwoord, EersteKeerAanmeldenVM.Password);
            Assert.Equal(Wachtwoord, EersteKeerAanmeldenVM.ConfirmPassword);
            _jobcoachRepository.Verify(m => m.Save(), Times.Once);

        }
        #endregion

        #region Index
        [Fact(Skip = "Not implemented yet")]
        public void Index_()
        {
            
        }
        #endregion

        #region NieuweAnalyse
        [Fact(Skip = "Not implemented yet")]
        public void NieuweAnalyse()
        {
            
        }
        #endregion
    }
}
